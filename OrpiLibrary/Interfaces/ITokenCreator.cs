using System.Collections.Generic;
using System.Security.Claims;
using OrpiLibrary.Models;

namespace OrpiLibrary.Interfaces {
    public interface ITokenCreator {
        public string CreateToken(TokenData data, IEnumerable<Claim> claims);
    }
}