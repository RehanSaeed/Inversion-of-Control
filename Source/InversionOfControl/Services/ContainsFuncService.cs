namespace InversionOfControl.Services
{
    using System;

    public class ContainsFuncService : IContainsFuncService
    {
        private readonly Func<IDisposableTransientService> transientServiceFactory;

        public ContainsFuncService(Func<IDisposableTransientService> transientServiceFactory) =>
            this.transientServiceFactory = transientServiceFactory;

        public void CreateInstances()
        {
            for (var i = 0; i < 10; i++)
            {
                // Don't pass arguments via the factory method, use properties or methods!
                using (var transientService = this.transientServiceFactory())
                {
                    // Do something
                }
            }
        }
    }
}
