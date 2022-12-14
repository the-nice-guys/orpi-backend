using OrpiLibrary.Models;

namespace OrpiLibrary.Interfaces {
    public interface ITokenLifeTimeManager {
        public double GetTimeBeforeExpiration(string token, TokenData data);
    }
}