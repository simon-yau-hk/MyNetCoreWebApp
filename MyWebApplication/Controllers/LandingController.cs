using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MyWebApplication.Controllers
{
    //not working now...
    [ApiController]
    [Route("api/Landing")]
    public class LandingController : Controller
    {
        private readonly IConfiguration _configuration;
        public LandingController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Log.Error("time: " + DateTime.Now.ToString());
            return Ok(new { message = "Welcome to the Landing page!" });
            //return View();
        }

        [HttpGet]
        [Route("Version")]
        public string Version()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "version.json");
            VersionInfo versionInfo = JsonSerializer.Deserialize<VersionInfo>(
                  System.IO.File.ReadAllText(path)
              );
            return versionInfo.Version+", " + _configuration["environment"] ;
        }

        public class VersionInfo
        {
            public string Version { get; set; }
           
        }






    }
}
