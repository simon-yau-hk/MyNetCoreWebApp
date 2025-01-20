using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyWebApplication.Model;
using MyWebApplication.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyWebApplication.Services
{
    public interface ITestService
    {
        string get();

    }

    public interface ITestDummyService : ITestService
    {

    }

    public interface ITestDummyAnotherService : ITestService
    {

    }
    public class TestService : ITestService
    {
        
        public string get()
        {
         
            return "TestService";
        }
    }

    public class TestAnotherService : ITestService
    {

        public string get()
        {

            return "TestAnotherService";
        }
    }

    public class TestDummyService : ITestDummyService
    {

        public string get()
        {

            return "TestDummyService";
        }
    }

    public class TestDummyAnotherService : ITestDummyAnotherService
    {

        public string get()
        {

            return "TestDummyAnotherService";
        }
    }
}
