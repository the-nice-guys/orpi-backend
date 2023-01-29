namespace OrpiLibrary.Models.Common;

public class Response<T> where T : struct {
    public Response(Guid guid, T result, string? message = null) {
        Guid = guid;
        Result = result;
        Message = message;
    }

    public Guid Guid { get; }
    public T Result { get; }
    public string? Message { get; }
}