using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            CookieContainer cc = new CookieContainer();

            cc.Add(new Uri("http://a.com/path1"), new Cookie("a1", "1"));
            cc.Add(new Uri("http://a.com/path2"), new Cookie("a2", "1"));
            cc.Add(new Uri("http://a.com/path3"), new Cookie("a3", "1"));


            cc.Add(new Uri("http://a.com/"), new Cookie("a1", "2"));

            cc.Add(new Uri("http://sub.a.com/"), new Cookie("a1", "3"));


            Console.WriteLine("http://a.com/ => " + cc.GetCookieHeader(new Uri("http://a.com/")));
            Console.WriteLine("http://a.com/path1 => " + cc.GetCookieHeader(new Uri("http://a.com/path1")));

            Console.WriteLine("http://sub.a.com/ => " + cc.GetCookieHeader(new Uri("http://sub.a.com/")));


            Console.WriteLine("http://a.com/ => " + FormatCookies(cc.GetCookies(new Uri("http://a.com/"))));


        }

        static string FormatCookies(CookieCollection cookieCollection)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Cookie item in cookieCollection)
            {
                sb.AppendFormat(item.Domain + " =>" + item.ToString() + "; ");
            }

            return sb.ToString();
        }
    }
}
