namespace CommunicationServiceApiFramework.Models;

internal class RequestModel
{
    public string? TypeName { get; set; }
    public string MethodName { get; set; }
    public string? Args { get; set; }
}