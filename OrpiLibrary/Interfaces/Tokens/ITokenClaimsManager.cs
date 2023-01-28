using System.Security.Claims;
using OrpiLibrary.Models.Common.Tokens;

namespace OrpiLibrary.Interfaces.Tokens {
    public interface ITokenClaimsManager {
        public IEnumerable<Claim> GetClaims(string token, TokenData data);
    }
}