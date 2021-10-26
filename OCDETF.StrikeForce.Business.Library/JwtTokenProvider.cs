using Microsoft.IdentityModel.Tokens;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OCDETF.StrikeForce.Business.Library
{
    public class JwtTokenProvider
    {
        private AppConfiguration myConfig { get; set; }
        public JwtTokenProvider(AppConfiguration appConfig)
        {
            myConfig = appConfig;
        }

        public string generateJwtToken(User user)
        {
            ClaimsIdentity claims = new ClaimsIdentity();
            //claims.AddClaim(new Claim("Email", user.Email));
            //claims.AddClaim(new Claim("StrikeForceName", user.StrikeForceName));
            if (user.Administrator)
                claims.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));
            if (user.Owner)
                claims.AddClaim(new Claim(ClaimTypes.Role, "Owner"));
            if (user.Contributor)
                claims.AddClaim(new Claim(ClaimTypes.Role, "Contributor"));

            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));


            
            //claims.AddClaim(new Claim("StrikeForceName", user.StrikeForceName));
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(myConfig.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
