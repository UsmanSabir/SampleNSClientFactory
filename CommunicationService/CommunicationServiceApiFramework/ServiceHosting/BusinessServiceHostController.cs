using System.Text.Json;
using System.Text.Json.Nodes;
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

            object?[]? parm = null;
            if (!string.IsNullOrWhiteSpace(input.Args))
            {
                //parm = Newtonsoft.Json.JsonConvert.DeserializeObject<object[]>(input.Args);
                parm = JsonSerializer.Deserialize<object[]>(input.Args);

                var parameterInfos = methodInfo.GetParameters();
                if (parm!.Length != parameterInfos.Length)
                    return Ok(ResponseModel.ErrorResult("Parameters count not matched"));

                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    JsonElement obj = (JsonElement)(parm[i] ?? throw new ArgumentNullException(nameof(input.Args)));
                    var rawText = obj.GetRawText();
                    var deserialize = JsonSerializer.Deserialize(rawText, parameterInfos[i].ParameterType);
                    parm[i] = deserialize;
                }
            }

            var response = methodInfo.Invoke(_instance, parm);
            var result = ResponseModel.SuccessResult(response);
            return Ok(result);
        }
        catch (Exception e)
        {
            return Ok(ResponseModel.ErrorResult(e.ToString()));
        }
    }
}