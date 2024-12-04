using System;
using System.Collections.Generic;

namespace MyWebApplication.Model
{
    public class TokenRequestModel
    {
        public string Token { get; set; }
        
        public string RefreshToken { get; set; }
    }

    public class AuthResultModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; } // 连接到 ASP.Net Identity User Id
        public string Token { get; set; }  // Refresh Token
        public string JwtId { get; set; } // 使用 JwtId 映射到对应的 token
        public bool IsUsed { get; set; } // 如果已经使用过它，我们不想使用相同的 refresh token 生成新的 JWT token
        public bool IsRevorked { get; set; } // 是否出于安全原因已将其撤销
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; } // refresh token 的生命周期很长，可以持续数月

   
    }


    public class MeetUpModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }  
        public string Image { get; set; } 
        public string Address { get; set; } 

    }
    


}
