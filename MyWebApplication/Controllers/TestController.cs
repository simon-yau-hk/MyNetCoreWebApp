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
    [Route("api/test")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;
        private readonly ITestDummyService _testDummyService;
        private readonly ITestDummyAnotherService _testDummyAnotherService;
        private readonly ITestService _testChosnService;
        private readonly TestService _testServiceImp;
        private readonly TestAnotherService _testAnotherServiceImp;
        public TestController(ITestService testService, TestService testServiceImp, 
            TestAnotherService testAnotherServiceImp, ITestDummyService testDummyService, ITestDummyAnotherService testDummyAnotherService)
        {
            this._testService = testService;
            this._testServiceImp = testServiceImp;
            this._testAnotherServiceImp = testAnotherServiceImp;
            this._testDummyService = testDummyService;
            this._testDummyAnotherService = testDummyAnotherService;
            this._testChosnService = testDummyAnotherService;
        }

        [HttpGet]
        [Route("")]
        public string Index()
        {
            var a = _testService.get();
            a = _testServiceImp.get();
            a = _testAnotherServiceImp.get();
            a = _testDummyService.get();
            a = _testDummyAnotherService.get();
            a = _testChosnService.get();
            return _testService.get();
        }

       

     


    }
}
