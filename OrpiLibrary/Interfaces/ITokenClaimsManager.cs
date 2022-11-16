using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace OrpiLibrary.Interfaces {
    public interface ITokenClaimsManager {
        public List<string> GetClaims(string token, TokenValidationParameters parameters);
    }
}