namespace CommunicationServiceApiFramework.Models;

public class RemoteException : ApplicationException
{
    public RemoteException(string error) : base(error)
    {

    }
}