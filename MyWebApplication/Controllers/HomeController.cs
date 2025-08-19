using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using MyWebApplication.Model;

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
        public string Index()
        {
            Log.Error("time: " + DateTime.Now.ToString());
            return DateTime.Now.ToString();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public string Login(string userName, string password)
        {
            
            return _homeService.Login(userName, password);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("googleLogin")]
        public async Task<string> GoogleLogin(GoogleLoginRequest request)
        {
            return await _homeService.GoogleLogin(request.AccessToken);

        }




    }
}
