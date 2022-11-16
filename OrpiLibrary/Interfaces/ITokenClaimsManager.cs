using System.Collections.Generic;

namespace OrpiLibrary.Interfaces {
    public interface ITokenClaimsManager {
        public List<string> GetClaims(string token);
    }
}