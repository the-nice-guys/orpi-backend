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
                claims: GetIdentity(claims).Claims,
                notBefore: DateTime.Now.ToUniversalTime(),
                expires: DateTime.Now.AddMinutes(data.Lifetime).ToUniversalTime(),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(data.Signature)),
                    SecurityAlgorithms.HmacSha256)
           );     
           
           return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        
        private static ClaimsIdentity GetIdentity(params string[] data) {
            List<Claim> claims = new();
            if (data.Length >= 1) 
                claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, data[0]));
            
            if (data.Length >= 2) 
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, data[1]));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            
            return claimsIdentity;
        }
    }
}