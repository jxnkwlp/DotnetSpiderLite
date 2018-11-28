using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Pipeline.Database.Dapper
{
    public class DapperDatabaseStore : IDatabaseStore
    {
        public Func<IList<IDictionary<string, object>>, bool> OnSave { get; set; }

        public virtual bool Save(IList<IDictionary<string, object>> data)
        {
            if (OnSave != null)
                return OnSave(data);

            return true;
        }
    }
}
