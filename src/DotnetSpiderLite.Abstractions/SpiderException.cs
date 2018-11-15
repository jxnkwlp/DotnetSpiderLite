using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  Exception
    /// </summary>
    public class SpiderException : Exception
    {
        public SpiderException() : base()
        {
        }

        public SpiderException(string message) : base(message)
        {
        }

        public SpiderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
