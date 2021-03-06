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
            // Static (fast)

            // Create an instance using the new keyword.
            var program1 = new Program();

            // Create an instance without calling the constructor.
            var program2 = FormatterServices.GetUninitializedObject(typeof(Program));

            // Dynamic (slow)

            // Create an instance using reflection.
            var program3 = typeof(Program).GetConstructor(Array.Empty<Type>()).Invoke(null);

            // Create an instance using dynamically.
            var program4 = Activator.CreateInstance<Program>();

            // Runtime Compiled (medium speed)

            // Create an instance using expression trees.
            var createProgram1 = Expression.Lambda<Func<Program>>(Expression.New(typeof(Program))).Compile();
            var program5 = createProgram1();

            // Writing low level IL code to new up an instance
            var constructor = typeof(Program).GetConstructor(Array.Empty<Type>());
            var dynamicMethod = new DynamicMethod("CreateProgram", typeof(Program), parameterTypes: null);
            var il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);
            var createProgram2 = (Func<Program>)dynamicMethod.CreateDelegate(typeof(Func<Program>));
            var program6 = createProgram2();

            // Pre-Compiled (fast)

            // Using T4 templates or new templating engine to generate design time code which uses the new keyword.
            // Not shown
        }
    }
}
