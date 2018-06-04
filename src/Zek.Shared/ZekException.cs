using System;

namespace Zek.Shared
{
    public class ZekException : Exception
    {
        public ZekException(string message) : base(message)
        {
        }

        public ZekException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}