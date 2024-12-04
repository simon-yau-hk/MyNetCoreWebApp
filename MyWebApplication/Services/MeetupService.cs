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
    public interface IMeetUpService
    {
        List<MeetUpModel> get();

    }
    public class MeetUpService : IMeetUpService
    {
        
        private readonly IConfiguration _configuration;
     
        public MeetUpService(IConfiguration configuration )
        {
            this._configuration = configuration;
        }

        public List<MeetUpModel> get()
        {
            var ret = new List<MeetUpModel>();
            
            for (int i = 1; i <= 3; i++)
            {
                var item = new MeetUpModel();
                item.Id = i;
                item.Address = i.ToString();
                item.Title = i.ToString();
                item.Image = i.ToString();
                item.Description = i.ToString();
                ret.Add(item);
            }

            return ret;
        }
    }
}
