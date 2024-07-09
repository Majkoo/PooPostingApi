﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PooPosting.Service.Models.Dtos.Picture;
using PooPosting.Service.Services.Helpers;
using PooPosting.Service.Services.Interfaces;

namespace PooPosting.Api.Controllers.Picture;

[ApiController]
[Authorize]
[Route("api/picture/{picId}")]
public class PictureUpdateController(
    IPictureService pictureService
    ) : ControllerBase
{
    [HttpPatch]
    [Route("name")]
    public async Task<IActionResult> UpdatePictureName(
        [FromRoute] string picId,
        [FromBody] UpdatePictureNameDto dto
    )
    {
        return Ok(await pictureService.UpdateName(IdHasher.DecodePictureId(picId), dto));
    }

    [HttpPatch]
    [Route("description")]
    public async Task<IActionResult> UpdatePictureDescription(
        [FromRoute] string picId,
        [FromBody] UpdatePictureDescriptionDto dto
    )
    {
        return Ok(await pictureService.UpdateDescription(IdHasher.DecodePictureId(picId), dto));
    }

    [HttpPatch]
    [Route("tags")]
    public async Task<IActionResult> UpdatePictureTags(
        [FromRoute] string picId,
        [FromBody] UpdatePictureTagsDto dto
    )
    {
        return Ok(await pictureService.UpdateTags(IdHasher.DecodePictureId(picId), dto));
    }
}