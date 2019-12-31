namespace InversionOfControl.Services
{
    public class SingletonOwningTransientService : ISingletonOwningTransientService
    {
        public SingletonOwningTransientService(ITransientService transientService) =>
            this.TransientService = transientService;

        public ITransientService TransientService { get; }
    }
}
