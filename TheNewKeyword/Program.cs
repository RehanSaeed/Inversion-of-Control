namespace TheNewKeyword
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.Serialization;

    public class Program
    {
        public static void Main()
        {
            var program1 = new Program();

            // Create an instance without calling the constructor.
            var program2 = FormatterServices.GetUninitializedObject(typeof(Program));

            var program3 = Activator.CreateInstance<Program>();

            var program4 = Factory<Program>.CreateInstance();

            // Writing low level IL code to new up an instance.

            // Using T4 templates or new templating engine to generate design time code which uses the new keyword.
        }

        public static class Factory<T>
            where T : new()
        {
            private static readonly Func<T> CreateInstanceFunc =
                Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();

            public static T CreateInstance() => CreateInstanceFunc();
        }
    }
}
