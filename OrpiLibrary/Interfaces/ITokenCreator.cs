using Microsoft.IdentityModel.Tokens;
using OrpiLibrary.Models;

namespace OrpiLibrary.Interfaces {
    public interface ITokenCreator {
        public string CreateToken(TokenData data, params string[] claims);
    }
}