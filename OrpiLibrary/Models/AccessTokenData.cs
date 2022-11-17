namespace OrpiLibrary.Models {
    public class AccessTokenData: TokenData {
        public AccessTokenData(): base(Config.AccessTokenSignature, Config.AccessTokenLifetime) {
        }
    }
}