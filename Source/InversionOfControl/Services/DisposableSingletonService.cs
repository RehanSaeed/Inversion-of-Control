namespace InversionOfControl.Services
{
    using System;

    public class DisposableSingletonService : Disposable, IDisposableSingletonService
    {
        protected override void DisposeManaged() =>
            Console.WriteLine($"{nameof(DisposableSingletonService)} disposed.{Environment.NewLine}");
    }
}
