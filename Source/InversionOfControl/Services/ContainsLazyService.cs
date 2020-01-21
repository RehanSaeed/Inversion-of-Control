namespace InversionOfControl.Services
{
    using System;

    public class ContainsLazyService : IContainsLazyService
    {
        public ContainsLazyService(Lazy<ITransientService> transientService) =>
            this.TransientService = transientService;

        public Lazy<ITransientService> TransientService { get; }

        public void UseLazyService()
        {
            Console.WriteLine($"{nameof(ITransientService)} instantiated.{Environment.NewLine}");
            var transientService = this.TransientService.Value;
            // Do something.
        }
    }
}
