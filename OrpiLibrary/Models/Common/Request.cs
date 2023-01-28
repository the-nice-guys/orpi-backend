namespace OrpiLibrary.Models.Common {
    public class Request<T> where T : struct {
        public Guid Guid { get; set; }
        public T Type { get; set; }
        public string Payload { get; set; } = null!;
    }
}