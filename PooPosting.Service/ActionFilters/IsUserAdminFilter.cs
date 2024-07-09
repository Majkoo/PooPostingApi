﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using PooPosting.Data.Exceptions;

namespace PooPosting.Service.ActionFilters;

public class IsUserAdminFilter: ActionFilterAttribute
{
    private const string Msg = "You have no rights to do that.";
    
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userRole = context.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Role);
        if ((userRole is null) || (userRole.Value != "3")) throw new ForbidException(Msg);
    }
}