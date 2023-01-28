using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrpiLibrary.Interfaces.Tokens;
using OrpiLibrary.Models.Common.Tokens;

namespace AuthenticationService.Services {
    public class TokenCreator: ITokenCreator {
        public string CreateToken(TokenData data, IEnumerable<Claim> claims) {
           var jwt = new JwtSecurityToken(
                issuer: data.Issuer,
                audience: data.Audience,
                claims: claims,
                notBefore: DateTime.Now.ToUniversalTime(),
                expires: DateTime.Now.AddMinutes(data.Lifetime).ToUniversalTime(),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(data.Signature)),
                    SecurityAlgorithms.HmacSha256)
           );     
           
           return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}