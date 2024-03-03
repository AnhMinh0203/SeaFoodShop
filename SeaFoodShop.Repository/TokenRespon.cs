using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace SeaFoodShop.Repository
{
    public class TokenRespon: ITokenRespon
    {
        private readonly IConfiguration _config;
        public TokenRespon(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(SignInModel account, Guid idUser)
        {
            // generate token that is valid for 15 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                   /* new Claim("phoneNumber", account.PhoneNumber.ToString()),*/
                    new Claim("userId", idUser.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidateJwtToken(string? token)
        {
            if (token == null) return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "userId").Value);

                // return account phonenumber from JWT token if validation successful
                return userId;
            }
            catch
            {
                return null;
            }
        }
/*
        public string TokenResult(string token)
        {
            TokenRespon tokenObject = new TokenRespon(_config);
            var idUser = tokenObject.ValidateJwtToken(token);
            var tokenValidate = tokenObject.ValidateJwtToken(token);
            if (tokenValidate == null)
            {
                return "Vui lòng đăng nhập tài khoản";
            }
            return "";
        }*/

        /*public SecurityToken ValidateToken(string token)
        {
            if (token == null) return null;
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
        }*/
    }
}
