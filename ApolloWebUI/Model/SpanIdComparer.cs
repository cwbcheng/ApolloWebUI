using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class SpanIdEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals([AllowNull] string x, [AllowNull] string y)
        {
            return x == y;
        }

        public int GetHashCode([DisallowNull] string obj)
        {
            if (obj == null)
            {
                return 0;
            }
            return obj.GetHashCode();
        }
    }
}
