using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class SpanIdOrderComparer : IComparer<string>
    {
        public int Compare([AllowNull] string x, [AllowNull] string y)
        {
            var a = x.Split('.');
            var b = y.Split('.');
            for (int i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                var temp = int.Parse(a[i]) - int.Parse(b[i]);
                if (temp == 0)
                {
                    continue;
                }
                else
                {
                    return temp;
                }
            }

            return a.Length - b.Length;
        }
    }
}
