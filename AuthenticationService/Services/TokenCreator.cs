using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrpiLibrary.Interfaces;

namespace AuthenticationService.Services {
    public class TokenCreator: ITokenCreator {
        private const string Issuer = "AuthenticationServer";
        private const string Audience = "ORPIClient";
        
        public string CreateToken(double lifetime, params string[] claims) {
           var jwt = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: GetIdentity(claims[0], claims[1]).Claims,
                notBefore: DateTime.Now.ToUniversalTime(),
                expires: DateTime.Now.AddMinutes(lifetime).ToUniversalTime(),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Hello")), 
                    SecurityAlgorithms.HmacSha256)
                );
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        
        private static ClaimsIdentity GetIdentity(string login, string role) {
            var claims = new List<Claim> {
                new (ClaimsIdentity.DefaultNameClaimType, login),
                new (ClaimsIdentity.DefaultRoleClaimType, role)
            };
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", 
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            
            return claimsIdentity;
        }
    }
}