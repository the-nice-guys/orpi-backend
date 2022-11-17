namespace OrpiLibrary.Models {
    public class RefreshTokenData: TokenData {
        public RefreshTokenData() {
            Signature = Config.RefreshSignature;
            Lifetime = Config.RefreshTokenLifetime;
        }
    }
}