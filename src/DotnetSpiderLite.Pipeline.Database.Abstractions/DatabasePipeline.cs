using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Pipeline.Database
{
    public class DatabasePipeline : BasePipeline
    {
        private readonly IDatabaseStore _store;

        public DatabasePipeline(IDatabaseStore store)
        {
            _store = store;
        }

        public override void Process(IList<ResultItems> resultItems)
        {
            IList<IDictionary<string, object>> list = new List<IDictionary<string, object>>();

            foreach (var item in resultItems)
            {
                list.Add(item);
            }

            if (list.Count > 0)
                _store.Save(list);

            // TODO 保存失败后，是否重试？

        }
    }
}
