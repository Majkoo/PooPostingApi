﻿using Microsoft.AspNetCore.Mvc;
using PooPosting.Api.Services.Interfaces;

namespace PooPosting.Api.Controllers.Picture;

[ApiController]
[Route("api/picture/popular")]
public class PopularController : ControllerBase
{
    private readonly IPopularService _popularService;

    public PopularController(
        IPopularService popularService)
    {
        _popularService = popularService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPopularContent()
    {
        return Ok(await _popularService.Get());
    }
}