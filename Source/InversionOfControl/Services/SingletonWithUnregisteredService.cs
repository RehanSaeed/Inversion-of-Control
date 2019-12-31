namespace InversionOfControl.Services
{
    public class SingletonWithUnregisteredService : ISingletonWithUnregisteredService
    {
        public SingletonWithUnregisteredService(IUnregisteredService unregisteredService)
        {
        }
    }
}
