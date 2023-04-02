using Microsoft.AspNetCore.Mvc;
using ModService.Models;
using ModService.Services;

namespace ModService.Controllers;

[ApiController]
[Route("[controller]")]
public class ModController : ControllerBase
{
    private readonly DbService _postService;
    public ModController(DbService postService)
    {
        this._postService = postService;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <returns>A list of all users.</returns>
    [HttpPost("{id}/flag")]
    public async Task<IActionResult> Flag(string id, Post post)
    {
        await this._postService.CreateFlagAsync(id, post);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPostsAsync()
    {
        var posts = await _postService.GetAllPostsAsync();

        return Ok(posts);
    }

    [HttpPost("{postId}/moderate")]
    public async Task<IActionResult> Moderate(string postId)
    {
        var post = await _postService.GetPostByIdAsync(postId);
        if (post == null)
        {
            return NotFound();
        }

        post.IsMod = true;
        if (await _postService.UpdatePostIsModAsync(postId)){
            return Ok();
        }
        else{
            return BadRequest();
        }
    }
}
