using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModService.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("timestamp")]
        public string Timestamp { get; set; }

        [BsonElement("parentid")]
        public string? ParentID { get; set; }

        [BsonElement("ismod")]
        public bool IsMod { get; set; }

        [BsonElement("flaggedby")]
        public string[] FlaggedBy { get; set; }
    }

}