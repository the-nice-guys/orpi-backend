using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using OrpiLibrary.Interfaces.Tokens;
using OrpiLibrary.Models.Common.Tokens;

namespace AuthenticationService.Services {
    public class TokenDataManager: ITokenClaimsManager, ITokenLifeTimeManager {
        public AccessTokenData AccessTokenData { get; }
        public RefreshTokenData RefreshTokenData{ get; }

        public TokenDataManager(AccessTokenData accessTokenData, RefreshTokenData refreshTokenData) {
            AccessTokenData = accessTokenData;
            RefreshTokenData = refreshTokenData;
        }

        public IEnumerable<Claim> GetClaims(string token, TokenData data) {
            return new JwtSecurityTokenHandler().ValidateToken(token, data.ValidationParameters, out _).Claims;
        }

        public double GetTimeBeforeExpiration(string token, TokenData data) {
            new JwtSecurityTokenHandler().ValidateToken(token, data.ValidationParameters, out var validatedToken);
            return (validatedToken.ValidTo.ToUniversalTime() - DateTime.Now.ToUniversalTime()).TotalMinutes;
        }
    }
}