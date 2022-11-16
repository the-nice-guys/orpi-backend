namespace OrpiLibrary.Interfaces {
    public interface ITokenCreator {
        public string CreateToken(double lifetime, params string[] claims);
    }
}