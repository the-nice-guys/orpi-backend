namespace OrpiLibrary.Models.Common {
    public class Response<T> where T : struct {
        public Guid Guid { get; set; }
        public T Result { get; set; }
        public string? Message { get; set; }
    }
}