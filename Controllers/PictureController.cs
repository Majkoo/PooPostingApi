﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PicturesAPI.Models;
using PicturesAPI.Models.Dtos;
using PicturesAPI.Services.Interfaces;

namespace PicturesAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/picture")]
public class PictureController : ControllerBase
{
    private readonly IPictureService _pictureService;
    private readonly IPictureLikingService _pictureLikingService;

    public PictureController(IPictureService pictureService, IPictureLikingService pictureLikingService)
    {
        _pictureService = pictureService;
        _pictureLikingService = pictureLikingService;
    }

    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    public ActionResult<PagedResult<PictureDto>> GetAllPictures([FromQuery] PictureQuery query)
    {
        var pictures = _pictureService.GetAll(query);
        return Ok(pictures);
    }
    
    [HttpGet]
    [EnableQuery]
    [AllowAnonymous]
    [Route("search")]
    public ActionResult<PagedResult<PictureDto>> SearchAllPictures([FromQuery] SearchQuery query)
    {
        var pictures = _pictureService.SearchAll(query);
        return Ok(pictures);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public IActionResult GetSinglePictureById([FromRoute] Guid id)
    {
        var picture = _pictureService.GetById(id);
        return Ok(picture);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id}/likes")]
    public IActionResult GetPictureLikes([FromRoute] Guid id)
    {
        var likes = _pictureService.GetPicLikes(id);
        return Ok(likes);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("{id}/likers")]
    public IActionResult GetPictureLikers([FromRoute] Guid id)
    {
        var likes = _pictureService.GetPicLikers(id);
        return Ok(likes);
    }

    [HttpPost]
    [Route("create")]
    public IActionResult PostPicture(
        [FromForm] IFormFile file, 
        [FromForm] string name, 
        [FromForm] string description, 
        [FromForm] string[] tags)
    {
        var dto = new CreatePictureDto()
        {
            Name = name,
            Description = description,
            Tags = tags.ToList()
        };
        
        var pictureId = _pictureService.Create(file, dto);
        
        return Created($"api/picture/{pictureId}", null);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult PutPictureUpdate([FromRoute] Guid id, [FromBody] PutPictureDto dto)
    {
        var result = _pictureService.Put(id, dto);
        return Ok(result);
    }

    [HttpPatch]
    [Route("{id}/voteup")]
    public IActionResult PatchPictureVoteUp([FromRoute] Guid id)
    {
        var result = _pictureLikingService.Like(id);
        return Ok(result);
    }
        
    [HttpPatch]
    [Route("{id}/votedown")]
    public IActionResult PatchPictureVoteDown([FromRoute] Guid id)
    {
        var result = _pictureLikingService.DisLike(id);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult DeletePicture([FromRoute] Guid id)
    {
        var result = _pictureService.Delete(id);
        return Ok(result);
    }

}