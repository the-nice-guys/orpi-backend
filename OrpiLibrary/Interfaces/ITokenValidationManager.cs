using Microsoft.IdentityModel.Tokens;

namespace OrpiLibrary.Interfaces {
    public interface ITokenValidationManager {
        public TokenValidationParameters GetTokenValidationParameters();
    }
}