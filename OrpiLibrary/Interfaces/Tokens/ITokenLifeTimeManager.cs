using OrpiLibrary.Models.Common.Tokens;

namespace OrpiLibrary.Interfaces.Tokens {
    public interface ITokenLifeTimeManager {
        public double GetTimeBeforeExpiration(string token, TokenData data);
    }
}