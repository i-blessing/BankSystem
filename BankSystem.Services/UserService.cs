using BankSystem.Business.Dto;
using BankSystem.Business.Exceptions;
using BankSystem.Data;
using BankSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Services
{
    public class UserService : IUserService
    {
        private readonly BankSystemDbContext _dbContext;

        /// <summary>
        ///  Secret
        /// </summary>
        /// <remarks>
        /// var hmac = new HMACSHA256();
        /// var key = Convert.ToBase64String(hmac.Key);
        /// </remarks>
        public const string Secret = "dw0YLYB7vKlmOgQq8DwGJAg81Hi4TeZSN9nyx2dfxuFPMQc2wiA1r2RwpQyFenJbnX3yYVKgvNNW+f1OzgV6xw==";

        public UserService(BankSystemDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (request.UserName?.ToLower() != "test" || request.Password != "password")
            {
                throw new BankSystemException(BankSystemExceptionStatusCodes.InvalidCredentials, "Invalid Credentials", BankSystemExceptionProperties.Create());
            }

            return new LoginResponse { Token = GenerateToken(request.UserName, 1) };
        }       

        public async Task<UserDto> GetUserForTokenAsync(string token)
        {
            if (!ValidateToken(token, out string username))
            {
                throw new BankSystemException(BankSystemExceptionStatusCodes.InvalidUserToken, "Invalid user token", BankSystemExceptionProperties.Create());
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());

            var props = BankSystemExceptionProperties.Create();

            props.Add("UserName", username);

            if (user == null)
            {
                throw new BankSystemException(BankSystemExceptionStatusCodes.InvalidUserNameInToken, "Invalid user name in token", props);
            }

            return new UserDto
            {
                UserId = user.Id,
                UserName = user.UserName,
            };
        }

        private static string GenerateToken(string username, long userId, int expireMinutes = 200)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        private static bool ValidateToken(string token, out string username)
        {
            username = String.Empty;
            
            var simplePrinciple = GetPrincipal(token);

            if (simplePrinciple == null) return false;

            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null || !identity.IsAuthenticated) return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value ?? String.Empty;

            if (string.IsNullOrEmpty(username)) return false;

            return true;
        }

        private static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}
