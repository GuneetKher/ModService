using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ModService.Models;
using System.Text;

namespace ModService.Services
{
    public class DbService
    {
        private readonly IMongoCollection<Post> _posts;
        private readonly ServiceAddresses _addresses;
        private readonly HttpClient _httpClient;
        public DbService(IDatabaseSettings settings, HttpClient httpClient, IOptions<ServiceAddresses> addresses)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _posts = database.GetCollection<Post>(settings.UsersCollectionName);
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(addresses.Value.PostServiceBaseUrl);
        }

        public async Task<IResult> CreateFlagAsync(string id, Post post)
        {
            var httpClient = new HttpClient();
            // var content = new StringContent(JsonConvert.SerializeObject(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"Post/flag/{post.Id}/{id}", null);
            if (!response.IsSuccessStatusCode)
            {
                return Results.Problem("Flagging failed");
            }
            await _posts.InsertOneAsync(post);
            return Results.Ok();
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            var posts = await _posts.Find(p => true).ToListAsync();
            return posts.OrderBy(p => p.IsMod).ToList();
        }

        public async Task<Post> GetPostByIdAsync(string id)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.Id, id);
            return await _posts.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePostIsModAsync(string id)
        {

            var httpClient = new HttpClient();
            // var content = new StringContent(JsonConvert.SerializeObject(), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"Post/{id}/ismod", null);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var filter = Builders<Post>.Filter.Eq(p => p.Id, id);
            var update = Builders<Post>.Update.Set(p => p.IsMod, true);
            var result = await _posts.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }
    }
}