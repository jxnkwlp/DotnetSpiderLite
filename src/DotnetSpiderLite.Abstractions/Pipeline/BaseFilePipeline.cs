using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotnetSpiderLite.Pipeline
{
    public abstract class BaseFilePipeline : BasePipeline
    {
        readonly string _target;

        public string DataFolder { get; set; }

        public BaseFilePipeline(string target)
        {
            _target = target;

            var root = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrEmpty(target))
            {
                DataFolder = Path.Combine(root, target);
            }
            else
            {
                DataFolder = root;
            }

            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }

        }


    }
}
