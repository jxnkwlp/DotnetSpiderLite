using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpiderLite.Pipeline.Database
{
    public interface IDatabaseStore
    {
        bool Save(IList<IDictionary<string, object>> data);
    }
}
