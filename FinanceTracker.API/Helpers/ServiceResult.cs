namespace FinanceTracker.API.Helpers;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data{ get; set; }
    public string ErrorMessage { get; set; }
    public string SuccessMessage { get; set; }

    public static ServiceResult<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data
    };

    public static ServiceResult<T> Success(string success) => new()
    {
        IsSuccess = true,
        SuccessMessage= success
    }; 

    public static ServiceResult<T> Failure(string error) => new()
    {
        IsSuccess = false,
        ErrorMessage = error
    }; 
}