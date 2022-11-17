using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OrpiLibrary.Interfaces;
using OrpiLibrary.Models;

namespace AuthenticationService.Services {
    public class TokenDataManager: ITokenClaimsManager, ITokenLifeTimeManager {
        public AccessTokenData AccessTokenData { get; }
        public RefreshTokenData RefreshTokenData{ get; }

        public TokenDataManager(AccessTokenData accessTokenData, RefreshTokenData refreshTokenData) {
            AccessTokenData = accessTokenData;
            RefreshTokenData = refreshTokenData;
        }

        public List<Claim> GetClaims(string token, TokenData data) {
            var handler = new JwtSecurityTokenHandler();
            var claims = handler.ValidateToken(token, data.ValidationParameters, out SecurityToken _).Claims.ToList();
            return claims;
        }

        public double GetTimeBeforeExpiration(string token, TokenData data) {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, data.ValidationParameters, out SecurityToken validatedToken);
            return (validatedToken.ValidTo.ToUniversalTime() - DateTime.Now.ToUniversalTime()).TotalMinutes;
        }
    }
}