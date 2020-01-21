namespace TheGenericHost.Services
{
    using System;

    public interface IClockService
    {
        DateTimeOffset Now { get; }
    }
}
