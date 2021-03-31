using System;

namespace RulersCourt.Models
{
    public class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance = new NoopDisposable();

        public void Dispose()
        {
        }
    }
}
