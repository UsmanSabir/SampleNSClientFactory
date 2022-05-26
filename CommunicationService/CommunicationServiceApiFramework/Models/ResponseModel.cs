namespace CommunicationServiceApiFramework.Models;

internal class ResponseModel
{
    public bool IsSuccess { get; set; }
    public string Error { get; set; }
    public string Response { get; set; }
}