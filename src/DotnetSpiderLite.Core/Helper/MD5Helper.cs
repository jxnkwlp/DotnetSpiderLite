using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DotnetSpiderLite.Helper
{
    internal static class MD5Helper
    {
        public static byte[] ComputeMD5(byte[] input)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(input);
            }
        }
    }
}
