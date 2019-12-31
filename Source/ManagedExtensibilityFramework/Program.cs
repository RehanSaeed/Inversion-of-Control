namespace ManagedExtensibilityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;

    public class Program
    {
        private readonly CompositionContainer container;

        public Program()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
            // catalog.Catalogs.Add(new DirectoryCatalog(@"C:\SomeDirectory"));

            this.container = new CompositionContainer(catalog);

            try
            {
                this.container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }

        [Import(typeof(ICalculator))]
        public ICalculator Calculator { get; set; }

        public static void Main()
        {
            var program = new Program();
            Console.WriteLine("Enter Command:");
            string command;
            while (true)
            {
                command = Console.ReadLine();
                Console.WriteLine(program.Calculator.Calculate(command));
            }
        }
    }
}
