namespace TheNewKeyword
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection.Emit;
    using System.Runtime.Serialization;

    public class Program
    {
        public static void Main()
        {
            // Create an instance using the new keyword (fast).
            var program1 = new Program();

            // Create an instance without calling the constructor.
            var program2 = FormatterServices.GetUninitializedObject(typeof(Program));

            // Create an instance using reflection (slow).
            var program3 = typeof(Program).GetConstructor(Array.Empty<Type>()).Invoke(null);

            // Create an instance using dynamically (slow).
            var program4 = Activator.CreateInstance<Program>();

            // Create an instance using expression trees (medium speed).
            var createProgram1 = Expression.Lambda<Func<Program>>(Expression.New(typeof(Program))).Compile();
            var program5 = createProgram1();

            // Writing low level IL code to new up an instance (medium speed).
            var constructor = typeof(Program).GetConstructor(Array.Empty<Type>());
            var dynamicMethod = new DynamicMethod("CreateProgram", typeof(Program), parameterTypes: null);
            var il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);
            var createProgram2 = (Func<Program>)dynamicMethod.CreateDelegate(typeof(Func<Program>));
            var program6 = createProgram2();

            // Using T4 templates or new templating engine to generate design time code which uses the new keyword (fast).
            // Not shown
        }
    }
}
