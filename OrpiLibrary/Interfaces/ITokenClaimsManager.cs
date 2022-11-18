using System.Collections.Generic;
using System.Security.Claims;
using OrpiLibrary.Models;

namespace OrpiLibrary.Interfaces {
    public interface ITokenClaimsManager {
        public IEnumerable<Claim> GetClaims(string token, TokenData data);
    }
}