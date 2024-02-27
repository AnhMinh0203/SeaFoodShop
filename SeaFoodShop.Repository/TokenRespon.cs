using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository
{
    public class TokenRespon
    {
        private readonly IConfiguration _config;
        public TokenRespon(IConfiguration config)
        {
            _config = config;
        }
        public SecurityToken ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"], // Issuer cần phải khớp với Issuer trong token
                ValidateAudience = true,
                ValidAudience = _config["Jwt:Audience"], // Audience cần phải khớp với Audience trong token
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return validatedToken;
        }
    }
}
