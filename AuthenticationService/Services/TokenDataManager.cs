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
        private readonly AccessTokenData _accessTokenData;
        private readonly RefreshTokenData _refreshTokenData;

        public TokenDataManager(AccessTokenData accessTokenData, RefreshTokenData refreshTokenData) {
            _accessTokenData = accessTokenData;
            _refreshTokenData = refreshTokenData;
        }
        
        public TokenData GetAccessTokenData() {
            return _accessTokenData;
        }
        
        public TokenData GetRefreshTokenData() {
            return _refreshTokenData;
        }
        
        public List<Claim> GetClaims(string token, TokenData data) {
            var handler = new JwtSecurityTokenHandler();
            var claims = handler.ValidateToken(
                token, 
                data.GetTokenValidationParameters(),
                out SecurityToken _).Claims.ToList();

            return claims;
        }

        public double GetTimeBeforeExpiration(string token, TokenData data) {
            var handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(token, data.GetTokenValidationParameters(), out SecurityToken validatedToken);
            return (validatedToken.ValidTo.ToUniversalTime() - DateTime.Now.ToUniversalTime()).TotalMinutes;
        }
    }
}