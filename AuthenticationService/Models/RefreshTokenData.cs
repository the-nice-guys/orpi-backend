using Microsoft.IdentityModel.Tokens;
using OrpiLibrary.Models;

namespace AuthenticationService.Models {
    public class RefreshTokenData: TokenData {
        private const string Issuer = "AuthenticationServer";
        private const string Audience = "ORPIClient";
        
        public override TokenValidationParameters GetTokenValidationParameters() {
            throw new System.NotImplementedException();
        }
    }
}