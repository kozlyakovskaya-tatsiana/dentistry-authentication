namespace Application.Exceptions
{
    public class ServiceNotLoadedException : Exception
    {
        public ServiceNotLoadedException(string serviceName)
            : base($"Service {serviceName} is not loaded.")
        {
        }
    }
}
