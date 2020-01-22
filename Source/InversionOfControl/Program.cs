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
            Console.ForegroundColor = ConsoleColor.Green;

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

            GeneralGuidelines();

            Console.Read();
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

            // Could be good for performance because it delays the call to the constructor until necessary.
        }

        public static void ResolvingCollections(Container container)
        {
            container.Register<IFooService, FooService1>(Reuse.Singleton);
            container.Register<IFooService, FooService2>(Reuse.Singleton);
            container.Register<IContainsCollectionService, ContainsCollectionService>(Reuse.Singleton);

            var containsCollectionService = container.Resolve<IContainsCollectionService>();

            // Disparate parts of an application can provide an implementation.

            // It's often useful for IFooService to filter, group or order the collection of services injected into
            // your class. To do this, it can be helpful to add Order, Group or other properties to IFooService.

            // If IFooService needs some state to work on from the parent class IContainsCollectionService,
            // it's often useful to pass some metadata to a method on IFooService
        }

        public static void ResolvingFuncs(Container container)
        {
            container.Register<IContainsFuncService, ContainsFuncService>(Reuse.Singleton);

            var containsFuncService = container.Resolve<IContainsFuncService>();

            containsFuncService.CreateInstances();

            // Useful when you need to create N number of instances of a type on demand.

            // Be careful, you control the lifetime of the types.
        }

        public static void GeneralGuidelines()
        {
            Console.WriteLine(@"  _____                           _  _____       _     _      _ _                 ");
            Console.WriteLine(@" / ____|                         | |/ ____|     (_)   | |    | (_)                ");
            Console.WriteLine(@"| |  __  ___ _ __   ___ _ __ __ _| | |  __ _   _ _  __| | ___| |_ _ __   ___  ___ ");
            Console.WriteLine(@"| | |_ |/ _ \ '_ \ / _ \ '__/ _` | | | |_ | | | | |/ _` |/ _ \ | | '_ \ / _ \/ __|");
            Console.WriteLine(@"| |__| |  __/ | | |  __/ | | (_| | | |__| | |_| | | (_| |  __/ | | | | |  __/\__ \");
            Console.WriteLine(@" \_____|\___|_| |_|\___|_|  \__,_|_|\_____|\__,_|_|\__,_|\___|_|_|_| |_|\___||___/");
            Console.WriteLine("");

            Console.WriteLine("1. Don't pass the Container around in the app.");
            Console.WriteLine();
            Console.WriteLine("2. Avoid the service locator pattern.");
            Console.WriteLine();
            Console.WriteLine("3. If you're injecting more than 3-6 types into your class, your class is probably doing");
            Console.WriteLine("   too much and should be split up. Testing is also a nightmare.");
            Console.WriteLine();
            Console.WriteLine("4. IoC makes your code easier to test. However, you have to actually write the tests!");
            Console.WriteLine();
            Console.WriteLine("5. If you are using an IoC container to setup your tests, that's probably an integration");
            Console.WriteLine("   test and not a unit test.");
            Console.WriteLine();
            Console.WriteLine("6. If you are manually new'ing up classes and passing arguments to the constructor that");
            Console.WriteLine("   are governed by the IoC container, you're probably doing it wrong.");
            Console.WriteLine();
            Console.WriteLine("7. Think about the lifetimes of your types. Ensure that there is no conflicting");
            Console.WriteLine("   lifetime composition. Use validation if available.");
            Console.WriteLine();
        }
    }
}
