namespace InversionOfControl
{
    using System;
    using System.Linq;
    using DryIoc;
    using InversionOfControl.Services;

    public class Program
    {
        public static void Main()
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());

            RegisterTypes(container);

            ResolveSingletons(container);
            ResolveScoped(container);
            ResolveTransients(container);

            IoCLifetimeComposition(container);

            ContainerValidation(container);

            DisposableTransients(container);
            DisposableScoped(container);
            DisposableSingletons();

            ResolvingLazy(container);
            ResolvingCollections(container);
            ResolvingFuncs(container);
        }

        private static void RegisterTypes(Container container)
        {
            container.Register<ISingletonService, SingletonService>(Reuse.Singleton);
            container.Register<IScopedService, ScopedService>(Reuse.Scoped);
            container.Register<ITransientService, TransientService>(Reuse.Transient);
        }

        public static void ResolveSingletons(Container container)
        {
            var singleton1 = container.Resolve<ISingletonService>();
            var singleton2 = container.Resolve<ISingletonService>();
            Console.WriteLine($"Singletons Equal: {singleton1 == singleton2}");
            Console.WriteLine();
        }

        private static void ResolveScoped(Container container)
        {
            try
            {
                var scoped = container.Resolve<IScopedService>();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine();
            }

            using (var context = container.OpenScope("ScopeName"))
            {
                var scoped1 = context.Resolve<IScopedService>();
                var scoped2 = context.Resolve<IScopedService>();
                Console.WriteLine($"Scoped Equal: {scoped1 == scoped2}");
                Console.WriteLine();
            }
        }

        public static void ResolveTransients(Container container)
        {
            var transient1 = container.Resolve<ITransientService>();
            var transient2 = container.Resolve<ITransientService>();
            Console.WriteLine($"Transients Equal: {transient1 == transient2}");
            Console.WriteLine();
        }

        private static void IoCLifetimeComposition(Container container)
        {
            // Allowed Lifetime Composition in Inversion of Control
            //
            //                 |-----------------------------------|
            //                 |            Parent Type            |
            //                 |-----------|-----------|-----------|
            //                 | Transient |  Scoped   | Singleton |
            // C T |-----------|-----------|-----------|-----------|
            // h y | Transient |     ✔    |     ❌    |    ❌    |
            // i p |  Scoped   |     ✔    |     ✔    |    ❌     |
            // l e | Singleton |     ✔    |     ✔    |     ✔    |
            // d   |-----------|-----------|-----------|-----------|

            container.Register<ISingletonOwningTransientService, SingletonOwningTransientService>(Reuse.Singleton);
            var singletonOwningTransient = container.Resolve<ISingletonOwningTransientService>();

            // 😢 Sadly DryIoC's container validation does not catch this scenario.
            container.Validate();
        }

        public static void ContainerValidation(Container container)
        {
            container.Register<ISingletonWithUnregisteredService, SingletonWithUnregisteredService>(Reuse.Singleton);

            var containerExceptions = container.Validate();
            if (containerExceptions.Length > 0)
            {
                Console.WriteLine(string.Join(Environment.NewLine, containerExceptions.Select(x => x.Value.Message)));
                Console.WriteLine();
            }
        }

        public static void DisposableTransients(Container container)
        {
            container.Register<IDisposableTransientService, DisposableTransientService>(Reuse.Transient);

            var disposableTransient = container.Resolve<IDisposableTransientService>();

            #region Answer
            disposableTransient.Dispose();
            #endregion
        }

        public static void DisposableScoped(Container container)
        {
            container.Register<IDisposableScopedService, DisposableScopedService>(Reuse.Scoped);

            #region Answer
            using (var context = container.OpenScope("DisposableScoped"))
            {
                var disposableScoped = context.Resolve<IDisposableScopedService>();
            }
            #endregion
        }

        public static void DisposableSingletons()
        {
            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
            container.Register<IDisposableSingletonService, DisposableSingletonService>(Reuse.Singleton);

            var disposableSingleton = container.Resolve<IDisposableSingletonService>();

            #region Answer
            container.Dispose();
            #endregion
        }

        public static void ResolvingLazy(Container container)
        {
            container.Register<IContainsLazyService, ContainsLazyService>(Reuse.Singleton);

            var containsLazyService = container.Resolve<IContainsLazyService>();

            containsLazyService.UseLazyService();
        }

        public static void ResolvingCollections(Container container)
        {
            container.Register<IFooService, FooService1>(Reuse.Singleton);
            container.Register<IFooService, FooService2>(Reuse.Singleton);
            container.Register<IContainsCollectionService, ContainsCollectionService>(Reuse.Singleton);

            var containsCollectionService = container.Resolve<IContainsCollectionService>();
        }

        public static void ResolvingFuncs(Container container)
        {
            container.Register<IContainsFuncService, ContainsFuncService>(Reuse.Singleton);

            var containsFuncService = container.Resolve<IContainsFuncService>();

            containsFuncService.CreateInstances();
        }
    }
}
