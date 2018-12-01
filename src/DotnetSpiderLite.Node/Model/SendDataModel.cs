using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Node.Model
{
    /// <summary>
    ///  发送数据
    /// </summary>
    public class SendDataModel
    {
        /// <summary>
        ///  类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        ///  是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        ///  数据
        /// </summary>
        public string Content { get; set; }

    }
}
