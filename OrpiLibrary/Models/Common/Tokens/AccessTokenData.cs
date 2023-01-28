namespace OrpiLibrary.Models.Common.Tokens {
    public class AccessTokenData: TokenData {
        public AccessTokenData(): base(Config.AccessTokenSignature, Config.AccessTokenLifetime) {
        }
    }
}