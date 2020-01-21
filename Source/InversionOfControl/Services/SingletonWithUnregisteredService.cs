namespace InversionOfControl.Services
{
    public class SingletonWithUnregisteredService : ISingletonWithUnregisteredService
    {
        private readonly IUnregisteredService unregisteredService;

        public SingletonWithUnregisteredService(IUnregisteredService unregisteredService) =>
            this.unregisteredService = unregisteredService;
    }
}
