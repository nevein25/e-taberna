namespace Auth.Application.DTOs;
public class Respons<T>
{
    public bool Status { get; set; }
    public string Message { get; set; } = default!;
    public T Data { get; set; } = default!;
    public List<string> Errors { get; set; } = [];
}