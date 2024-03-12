namespace FitJarus.Models;

public class ApiResponse<T> : ApiResponse
{
    public T Content { get; set; }

    public ApiResponse()
    {
        Success = false;
        Error = string.Empty;
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Error { get; set; }

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
