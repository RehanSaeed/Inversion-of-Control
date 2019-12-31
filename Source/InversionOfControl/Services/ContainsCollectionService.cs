namespace InversionOfControl.Services
{
    using System.Collections.Generic;

    public class ContainsCollectionService : IContainsCollectionService
    {
        public ContainsCollectionService(
            IEnumerable<IFooService> fooServicesEnumerable,
            IFooService[] fooServicesArray,
            IReadOnlyCollection<IFooService> fooServicesCollection)
        {
            this.FooServicesEnumerable = fooServicesEnumerable;
            this.FooServicesArray = fooServicesArray;
            this.FooServicesCollection = fooServicesCollection;
        }

        public IEnumerable<IFooService> FooServicesEnumerable { get; }

        public IFooService[] FooServicesArray { get; }

        public IReadOnlyCollection<IFooService> FooServicesCollection { get; }
    }
}
