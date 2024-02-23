namespace FitJarus.Models;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public T Content { get; set; }

    public ApiResponse()
    {
        Success = false;
        Error = string.Empty;
    }

    public void Ok()
    {
        Success = true;
    }
}
