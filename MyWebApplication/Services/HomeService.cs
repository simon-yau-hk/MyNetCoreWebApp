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
    public interface IHomeService
    {
         void Test();

        string Login(string userName, string password);
        AuthResultModel VerifyAndGenerateToken(TokenRequestModel tokenRequest);
    }
    public class HomeService: IHomeService
    {
        
        private readonly IHomeRepository _homeRepository;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParams = new TokenValidationParameters();
        public HomeService(IHomeRepository homeRepository,IConfiguration configuration )
        {
            this._homeRepository = homeRepository;
            this._configuration = configuration;
        }
        public void Test()
        {
            var i = _homeRepository.Get();
        }

        public string Login(string userName, string password)
        {
            if (userName == "aaa" && password == "111")
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Email, userName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                return stringToken;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public AuthResultModel VerifyAndGenerateToken(TokenRequestModel tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validation 1 - Validation JWT token format
                // 此验证功能将确保 Token 满足验证参数，并且它是一个真正的 token 而不仅仅是随机字符串
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

                // Validation 2 - Validate encryption alg
                // 检查 token 是否有有效的安全算法
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        throw new Exception();
                    }
                }

                // Validation 3 - validate expiry date
                // 验证原 token 的过期时间，得到 unix 时间戳
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                {
                    "Token has not yet expired"
                }
                    };
                }

                // validation 4 - validate existence of the token
                // 验证 refresh token 是否存在，是否是保存在数据库的 refresh token
                RefreshToken storedRefreshToken = new RefreshToken();//testing only
                //var storedRefreshToken = await _apiDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                if (storedRefreshToken == null)
                {
                    return new AuthResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                {
                    "Refresh Token does not exist"
                }
                    };
                }

                // Validation 5 - 检查存储的 RefreshToken 是否已过期
                // Check the date of the saved refresh token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                {
                    return new AuthResultModel()
                    {
                        Errors = new List<string>() { "Refresh Token has expired, user needs to re-login" },
                        Success = false
                    };
                }

                // Validation 6 - validate if used
                // 验证 refresh token 是否已使用
                if (storedRefreshToken.IsUsed)
                {
                    return new AuthResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                {
                    "Refresh Token has been used"
                }
                    };
                }

                // Validation 7 - validate if revoked
                // 检查 refresh token 是否被撤销
                if (storedRefreshToken.IsRevorked)
                {
                    return new AuthResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                {
                    "Refresh Token has been revoked"
                }
                    };
                }

                // Validation 8 - validate the id
                // 这里获得原 JWT token Id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                // 根据数据库中保存的 Id 验证收到的 token 的 Id
                if (storedRefreshToken.JwtId != jti)
                {
                    return new AuthResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                {
                    "The token doesn't mateched the saved token"
                }
                    };
                }

                // update current token 
                // 将该 refresh token 设置为已使用
                storedRefreshToken.IsUsed = true;
                // _apiDbContext.RefreshTokens.Update(storedRefreshToken);
                //await _apiDbContext.SaveChangesAsync();

                // 生成一个新的 token
                //var dbUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
                return GenerateJwtToken("aaa", "aaa@a.com");


            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {
                    return new AuthResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                {
                    "Token has expired please re-login"
                }
                    };
                }
                else
                {
                    return new AuthResultModel()
                    {
                        Success = false,
                        Errors = new List<string>()
                {
                    "Something went wrong."
                }
                    };
                }
            }
        }

        private  AuthResultModel GenerateJwtToken(string userName, string email)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
                Expires = DateTime.UtcNow.AddSeconds(30), // 比较合理的值为 5~10 分钟，这里设置 30 秒只是作演示用
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                //UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(25) + Guid.NewGuid()
            };

            //await _apiDbContext.RefreshTokens.AddAsync(refreshToken);
            //await _apiDbContext.SaveChangesAsync();

            return new AuthResultModel()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTimeVal;
        }

    }
}
