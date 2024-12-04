using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MyWebApplication.Model;

namespace MyWebApplication.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/meetup")]
    public class MeetUpController : Controller
    {
        private readonly IMeetUpService _MeetUpService;
        public MeetUpController(IMeetUpService MeetUpService)
        {
            this._MeetUpService = MeetUpService;
        }

        [HttpGet]
        [Route("")]
        public List<MeetUpModel> Index()
        {
            return _MeetUpService.get();
        }

       

     


    }
}
