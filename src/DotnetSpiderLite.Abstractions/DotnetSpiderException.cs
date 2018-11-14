using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite
{
    /// <summary>
    ///  Exception
    /// </summary>
    public class DotnetSpiderException : Exception
    {
        public DotnetSpiderException() : base()
        {
        }

        public DotnetSpiderException(string message) : base(message)
        {
        }

        public DotnetSpiderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
