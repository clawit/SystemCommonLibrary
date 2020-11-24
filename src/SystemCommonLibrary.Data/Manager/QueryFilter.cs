using System;
using System.Collections.Generic;
using System.Text;

namespace SystemCommonLibrary.Data.Manager
{
    public class QueryFilter
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public QueryComparison Comparison { get; set; }

        public QueryOperator Operator { get; set; }
        public List<QueryFilter> SubFilters { get; set; }
    }
}
