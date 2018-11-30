using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Node
{
    public class NodeClientCommand
    {
        /// <summary>
        ///  0: 空闲
        ///  1: 退出
        ///  10： 新的request
        /// </summary>
        public int Command { get; set; }

        public string CommandData { get; set; }

    }
}
