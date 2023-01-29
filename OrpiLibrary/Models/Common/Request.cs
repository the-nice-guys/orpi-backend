namespace OrpiLibrary.Models.Common;

public class Request<T> where T : struct {
    public Request(Guid guid, T type, string payload) {
        Guid = guid;
        Type = type;
        Payload = payload;
    }
    public Guid Guid { get; }
    public T Type { get; }
    public string Payload { get; }
}