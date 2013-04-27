using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Salary.Core.Utility
{
    public class UrlParamBuilder : CollectionBase
    {
        public UrlParamBuilder()
        {
        }

        public UrlParamBuilder(String host)
            : this()
        {
            Host = host;
        }

        public String Host { get; set; }

        public void AppendItem(String name, Object value)
        {
            base.List.Add(new KeyValuePair<String, Object>(name, value));
        }

        public Boolean IsEmpty
        {
            get
            {
                return base.Count == 0;
            }
        }

        public String ToUrlString()
        {
            if (String.IsNullOrEmpty(Host))
            {
                return ToString();
            }
            else
            {
                return String.Concat(Host, "?", ToString());
            }
        }

        public override String ToString()
        {
            var list = base.List.OfType<KeyValuePair<String, Object>>().Select(i => String.Format("{0}={1}", i.Key, i.Value));
            return list.Count() > 0 ? list.Aggregate((pred, next) => String.Format("{0}&{1}", pred, next)) : String.Empty;
        }
    }
}
