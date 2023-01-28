using System.Security.Claims;
using OrpiLibrary.Models.Common.Tokens;

namespace OrpiLibrary.Interfaces.Tokens {
    public interface ITokenCreator {
        public string CreateToken(TokenData data, IEnumerable<Claim> claims);
    }
}