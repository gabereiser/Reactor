using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
    internal class ArrayMultipleIndexFilter : PathFilter
    {
        internal List<int> Indexes;

        public ArrayMultipleIndexFilter(List<int> indexes)
        {
            Indexes = indexes;
        }

        public override IEnumerable<JToken> ExecuteFilter(JToken root, IEnumerable<JToken> current,
            bool errorWhenNoMatch)
        {
            foreach (var t in current)
            foreach (var i in Indexes)
            {
                var v = GetTokenIndex(t, errorWhenNoMatch, i);

                if (v != null) yield return v;
            }
        }
    }
}