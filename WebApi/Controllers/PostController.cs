using Application.Common.Interfaces;
using Application.CQRS.Posts.DTOs;
using Application.CQRS.Posts.Handlers;
using Common.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public PostController( IMediator mediator , IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;

    }


    [HttpPost("CreatePost")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostHandler.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsSuccess)
        {
            return CreatedAtAction(nameof(GetPostById), new { id = response.Data.Id }, response.Data);
        }


        //brat mende bele de bir problem var ki exceptionlar custom deil e mende commondan gelir yenui ki ...
        return BadRequest(response.Errors);
    }


    [HttpGet("GetPostById/{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var response = await _mediator.Send(new GetPostByIdHandler.Command(id));

        if (response.IsSuccess)
        {
            return Ok(response.Data);
        }

        return NotFound(response.Errors);
    }


    [HttpGet("PostList")]
    public async Task<IActionResult> GetPostsList()
    {
        var response = await _mediator.Send(new PostListHandler.Command());

        if (response.IsSuccess)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Errors);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> UpdatePost([FromQuery] UpdatePostHandler.Command command)
    {
        var response = await _mediator.Send(command);

        if (response.IsSuccess)
        {
            return Ok(response.Data);
        }

        return BadRequest(response.Errors);
    }

//qeyd : frombody yazanda error olur sebebi ise id ni hem route dan hemde body icersinden alir buda uygun gelmir error aliriq 
//query olanda  query string de olmayan property leri null kimi qebul edir ve xeta olmur ,, eger body dede hamsi nullable olsaydi problem olmazdi 


    [HttpDelete("Delete/{id}")]
    [Authorize]
    public async Task<IActionResult> DeletePost(int id, [FromQuery] string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new BadRequestException("Delete reason is required.");


        var userId = _userContext.MustGetUserId();
        var command = new DeletePostHandler.Command(id ,userId, reason);

        var result = await _mediator.Send(command);

        return Ok(result);


    }




}
