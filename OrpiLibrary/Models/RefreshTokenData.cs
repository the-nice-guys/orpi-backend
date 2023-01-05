namespace OrpiLibrary.Models {
    public class RefreshTokenData: TokenData {
        public RefreshTokenData(): base(Config.RefreshTokenSignature, Config.RefreshTokenLifetime) {
        }
    }
}