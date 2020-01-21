namespace ManagedExtensibilityFramework.Operations
{
    using System.ComponentModel.Composition;

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '+')]
    public class Add : IOperation
    {
        public int Operate(int left, int right) => left + right;
    }
}
