namespace Auth.Application.DTOs;
public class Response<T>
{
    public bool Status { get; set; }
    public string Message { get; set; } = default!;
    public T Data { get; set; } = default!;
    public List<string> Errors { get; set; } = [];

    public Response(T data, string message)
    {
        Status = true;
        Message = message;
        Data = data;
    }

    public Response(string message, params string[] errors)
    {
        Status = false;
        Message = message;
        Errors.AddRange(errors);
    }
}