using System.Reflection;
using CommunicationServiceAbstraction;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CommunicationServiceApiFramework.ServiceHosting;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class BusinessControllerNameConvention : Attribute, IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.GetGenericTypeDefinition() !=
            typeof(BusinessServiceHostController<>))
        {
            return;
        }

        var entityType = controller.ControllerType.GenericTypeArguments[0];
        var baseType = typeof(IBusinessService);
        var entityTypeInfo = entityType.GetTypeInfo();
        var derivedType = entityType;


        var interfaces = entityType.GetInterfaces(); //entityTypeInfo.ImplementedInterfaces
        var contract = interfaces.FirstOrDefault();
        if (contract != null)
        {
            if (baseType.IsAssignableFrom(contract))
                derivedType = contract;
            else
            {
                for (int i = 1; i < interfaces.Length; i++)
                {
                    contract = interfaces[i];
                    if (baseType.IsAssignableFrom(contract))
                    {
                        derivedType = contract;
                        break;
                    }
                }
            }
        }

        controller.ControllerName = $"{derivedType.Name.TrimStart('I')}Host";
    }
}