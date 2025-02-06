using System;

namespace Apollo77.Shared.Util;

public class Response<T>
{
    public T? Value { get; private set; }
    public bool Succsess { get; private set; }
    public string? Message { get; private set; }
    public string? ErrorId { get; set; }

    private Response(T? value, bool succsess, string? message, string? errorId)
    {
        this.Value = value;
        this.Succsess = succsess;
        this.Message = message;
        this.ErrorId = errorId;
    }

    public static Response<T> SetResponse(T? value, bool succsess, string? message, string? errorId)
    {
        return new Response<T>(value, succsess, message, errorId);
    }
}