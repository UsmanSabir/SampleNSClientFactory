using System.Collections.ObjectModel;
using System.Reflection;

namespace CommunicationServiceApiFramework.ServiceHosting;

public class BusinessServiceDiscovery
{
    private static readonly List<TypeInfo> RegisteredTypes = new List<TypeInfo>();

    public static ReadOnlyCollection<TypeInfo> GetBusinessServices()
    {
        return RegisteredTypes.AsReadOnly();
    }

    public static void RegisterType(TypeInfo ti)
    {
        RegisteredTypes.Add(ti);
    }
}