namespace OrpiLibrary.Models {
    public class AccessTokenData: TokenData {
        public AccessTokenData() {
            Signature = Config.AccessSignature;
            Lifetime = Config.AccessTokenLifetime;
        }
    }
}