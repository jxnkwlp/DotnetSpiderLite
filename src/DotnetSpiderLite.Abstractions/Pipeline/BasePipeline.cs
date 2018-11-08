﻿using System;
using System.Collections.Generic;
using System.Text;
using DotnetSpiderLite.Abstractions.Logs;

namespace DotnetSpiderLite.Abstractions.Pipeline
{
    public abstract class BasePipeline : IPipeline
    {
        public ILogger Logger { get; set; }

        /// <summary>
        ///  dispose when you need.
        /// </summary>
        public virtual void Dispose()
        {
        }

        public abstract void Process(IList<ResultItems> resultItems);


    }
}
