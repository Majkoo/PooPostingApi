﻿#nullable enable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using PooPosting.Application.Models.Queries;
using PooPosting.Application.Services;
using PooPosting.Application.Services.Helpers;

namespace PooPosting.Api.Controllers.Account;

[ApiController]
[Route("api/account")]
public class AccountController(AccountService accountService) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> SearchAllAccounts(
        [FromQuery] AccountQueryParams paginationParameters
    )
    {
        return Ok(await accountService.GetAll(paginationParameters));
    }

    [HttpGet]
    [Route("{accId}")]
    public async Task<IActionResult> GetAccountById(
        [FromRoute] string accId
        )
    {
        return Ok(await accountService.GetById(IdHasher.DecodeAccountId(accId)));
    }
    
    [HttpGet]
    [Authorize]
    [Route("me")]
    public async Task<IActionResult> GetCurrentAccount()
    {
        return Ok(await accountService.GetCurrent());
    }

    [HttpDelete]
    [Authorize]
    [Route("{accId}")]
    public async Task<IActionResult> DeleteAccount(
        [FromRoute] string accId
    )
    {
        await accountService.Delete(IdHasher.DecodeAccountId(accId));
        return Ok();
    }

}