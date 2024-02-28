﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NIHR.StudyManagement.Api.Controllers
{
    [Route("api/home")]
    public class HomeController : ApiControllerBase
    {
        [HttpGet]
        public IActionResult Welcome()
        {
            return Ok("Welcome to the Study Management API.");
        }

        [HttpGet]
        [Authorize]
        [Route("authenticated")]
        public IActionResult WelcomeAuthenticated()
        {
            return Ok("Welcome to the Study Management API - you're authenticated.");
        }
    }
}