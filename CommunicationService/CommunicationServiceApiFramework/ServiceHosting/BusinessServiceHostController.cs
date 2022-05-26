using System.Text.Json;
using CommunicationServiceAbstraction;
using CommunicationServiceApiFramework.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationServiceApiFramework.ServiceHosting;

[Route("api/[controller]")]
[ApiController]
[BusinessControllerNameConvention]
public class BusinessServiceHostController<T> : ControllerBase where T : IBusinessService
{
    private T _instance;

    public BusinessServiceHostController(T instance)
    {
        _instance = instance;
    }

    [HttpPost]
    public IActionResult Post([FromBody] RequestModel input)
    {
        try
        {
            var methodInfo = _instance.GetType().GetMethod(input.MethodName);
            if (methodInfo == null)
            {
                //todo log
                return Ok(ResponseModel.ErrorResult(
                    $"Method '{input?.MethodName}' not found on type '{input?.TypeName}'"));

            }
            //todo parameters
            object?[]? parm = null;
            if (!string.IsNullOrWhiteSpace(input.Args)) parm = JsonSerializer.Deserialize<object?[]>(input.Args);

            var response = methodInfo.Invoke(_instance, parm);
            var json = JsonSerializer.Serialize(response);
            var result = ResponseModel.SuccessResult(response);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Ok(ResponseModel.ErrorResult(e.ToString()));
        }
    }
}