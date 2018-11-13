using DotnetSpiderLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpiderLite
{
    public static class PageExtensions
    {
        //public static int GetRetryCount(this Page page)
        //{
        //    if (page.Extra.ContainsKey("PAGE_RETRY_COUNT"))
        //    {
        //        int.TryParse(page.Extra["PAGE_RETRY_COUNT"], out var count);
        //        return count;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //public static void IncrementRetryCount(this Page page)
        //{
        //    if (page.Extra.ContainsKey("PAGE_RETRY_COUNT"))
        //    {
        //        int.TryParse(page.Extra["PAGE_RETRY_COUNT"], out var count);
        //        page.Extra["PAGE_RETRY_COUNT"] = (count + 1).ToString();
        //    }
        //    else
        //    {
        //        page.Extra["PAGE_RETRY_COUNT"] = "1";
        //    }
        //}
    }
}
