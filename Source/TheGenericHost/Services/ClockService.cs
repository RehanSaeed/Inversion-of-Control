namespace TheGenericHost.Services
{
    using System;

    public class ClockService : IClockService
    {
        public DateTimeOffset Now => DateTimeOffset.Now;
    }
}
