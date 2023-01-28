namespace OrpiLibrary.Models.Common.Tokens {
    public class RefreshTokenData: TokenData {
        public RefreshTokenData(): base(Config.RefreshTokenSignature, Config.RefreshTokenLifetime) {
        }
    }
}