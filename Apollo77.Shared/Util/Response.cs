namespace Apollo77.Shared.Util;

public class Response<T>
{
    public T? Value { get; private set; }
    public bool Succsess { get; private set; }
    public string? Message { get; private set; }

    private Response(T? value, bool succsess, string? message)
    {
        this.Value = value;
        this.Succsess = succsess;
        this.Message = message;
    }

    public static Response<T> SetResponse(T? value, bool succsess, string? message)
    {
        return new Response<T>(value, succsess, message);
    }
}