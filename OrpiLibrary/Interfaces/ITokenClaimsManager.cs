using System.Collections.Generic;
using OrpiLibrary.Models;

namespace OrpiLibrary.Interfaces {
    public interface ITokenClaimsManager {
        public List<string> GetClaims(string token, TokenData data);
    }
}