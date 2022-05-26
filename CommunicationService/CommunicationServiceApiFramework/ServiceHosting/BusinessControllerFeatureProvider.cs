using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace CommunicationServiceApiFramework.ServiceHosting;

public class BusinessControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var types = BusinessServiceDiscovery.GetBusinessServices();
        // This is designed to run after the default ControllerTypeProvider, 
        // so the list of 'real' controllers has already been populated.
        foreach (var entityType in types)
        {
            var typeName = entityType.Name + "Controller";
            if (feature.Controllers.All(t => t.Name != typeName))
            {
                // There's no 'real' controller for this entity, so add the generic version.
                var controllerType = typeof(BusinessServiceHostController<>)
                    .MakeGenericType(entityType).GetTypeInfo();
                feature.Controllers.Add(controllerType);
            }
        }
    }
}