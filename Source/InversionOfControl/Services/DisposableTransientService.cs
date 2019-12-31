namespace InversionOfControl.Services
{
    using System;

    public class DisposableTransientService : Disposable, IDisposableTransientService
    {
        protected override void DisposeManaged() =>
            Console.WriteLine($"{nameof(DisposableTransientService)} disposed.{Environment.NewLine}");
    }
}
