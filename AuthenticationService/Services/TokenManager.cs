using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OrpiLibrary.Interfaces;
using OrpiLibrary.Models;

namespace AuthenticationService.Services {
    public class TokenManager: ITokenClaimsManager, ITokenLifeTimeManager {
        public List<string> GetClaims(string token, TokenData data) {
            var handler = new JwtSecurityTokenHandler();
            var claims = handler.ValidateToken(
                token, 
                data.GetTokenValidationParameters(),
                out SecurityToken _).Claims.ToList();

            return new List<string> {claims[0].Value, claims[1].Value};
        }

        public double GetTimeBeforeExpiration(string token, TokenData data) {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, data.GetTokenValidationParameters(), out SecurityToken validatedToken);
            return (validatedToken.ValidTo.ToUniversalTime() - DateTime.Now.ToUniversalTime()).TotalMinutes;
        }
    }
}