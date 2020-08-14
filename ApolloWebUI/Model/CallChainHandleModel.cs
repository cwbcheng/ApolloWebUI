using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class CallChainHandleModel
    {
        public long Id { get; set; }

        public string UserId { get; set; }

        public string TraceId { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }
    }
}
