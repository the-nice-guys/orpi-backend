using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrpiLibrary.Interfaces;
using OrpiLibrary.Models;

namespace AuthenticationService.Services {
    public class TokenCreator: ITokenCreator {
        public string CreateToken(TokenData data, params string[] claims) {
           var jwt = new JwtSecurityToken(
                issuer: data.Issuer,
                audience: data.Audience,
                claims: GetIdentity(claims[0], claims[1]).Claims,
                notBefore: DateTime.Now.ToUniversalTime(),
                expires: DateTime.Now.AddMinutes(data.Lifetime).ToUniversalTime(),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(data.Signature)),
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