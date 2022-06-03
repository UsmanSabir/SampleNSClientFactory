using System.Text.Json;

namespace CommunicationServiceApiFramework.Models;

public class ResponseModel
{
    public bool IsSuccess { get; set; }
    public string Error { get; set; }
    public string Response { get; set; }

    public static ResponseModel SuccessResult<T>(T result)
    {
        return new ResponseModel()
        {
            IsSuccess = true,
            Response = JsonSerializer.Serialize(result)
        };
    }

    public static ResponseModel ErrorResult(string result)
    {
        return new ResponseModel()
        {
            IsSuccess = false,
            Error = result
        };
    }
}