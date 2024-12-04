﻿using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace MyWebApplication.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/home")]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        public HomeController(IHomeService homeService)
        {
            this._homeService = homeService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            _homeService.Test();
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public string Login(string userName, string password)
        {
            
            return _homeService.Login(userName, password);
        }

     


    }
}
