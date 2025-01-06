using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace MyWebApplication.Controllers
{
    //not working now...
    [ApiController]
    [Route("Landing")]
    public class LandingController : Controller
    {
        public LandingController()
        {
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Log.Error("time: " + DateTime.Now.ToString());
            return Ok(new { message = "Welcome to the Landing page!" });
            //return View();
        }

     

     


    }
}
