namespace InversionOfControl.Services
{
    using System;

    public class DisposableScopedService : Disposable, IDisposableScopedService
    {
        protected override void DisposeManaged() =>
            Console.WriteLine($"{nameof(DisposableScopedService)} disposed.{Environment.NewLine}");
    }
}
