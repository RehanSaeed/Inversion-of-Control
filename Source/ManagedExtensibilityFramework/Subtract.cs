namespace ManagedExtensibilityFramework
{
    using System.ComponentModel.Composition;

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '-')]
    public class Subtract : IOperation
    {
        public int Operate(int left, int right) => left - right;
    }
}
