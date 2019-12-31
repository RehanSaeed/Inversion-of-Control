namespace ManagedExtensibilityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    [Export(typeof(ICalculator))]
    public class MySimpleCalculator : ICalculator
    {
        [ImportMany]
        public IEnumerable<Lazy<IOperation, IOperationData>> Operations { get; set; }

        public string Calculate(string input)
        {
            var fn = this.FindFirstNonDigit(input);
            if (fn < 0)
            {
                return "Could not parse command.";
            }

            if (!int.TryParse(input.Substring(0, fn), out var left) ||
                !int.TryParse(input.Substring(fn + 1), out var right))
            {
                return "Could not parse command.";
            }

            var operation = input[fn];
            foreach (var item in this.Operations)
            {
                if (item.Metadata.Symbol.Equals(operation))
                {
                    return item.Value.Operate(left, right).ToString();
                }
            }

            return "Operation Not Found!";
        }

        private int FindFirstNonDigit(string s)
        {
            for (var i = 0; i < s.Length; i++)
            {
                if (!char.IsDigit(s[i]))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
