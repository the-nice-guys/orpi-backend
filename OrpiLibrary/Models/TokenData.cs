using Microsoft.IdentityModel.Tokens;

namespace OrpiLibrary.Models {
    public abstract class TokenData {
        private string _issuer;
        private string _audience;
        private double _lifeTime;
        
        public abstract TokenValidationParameters GetTokenValidationParameters();
    }
}