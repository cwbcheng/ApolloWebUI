using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class CallChainModel
    {
        public List<CallChainRequest> Requests { get; set; }

        public List<CallChainResponse> Responses { get; set; }
    }

    public class CallChainRequest
    {
        public string TraceId { get; set; }

        public string SpanId { get; set; }

        public string Location { get; set; }

        public string Method { get; set; }

        public DateTime RequestTime { get; set; }

        public object Request { get; set; }
    }

    public class CallChainResponse
    {
        public string TraceId { get; set; }

        public string SpanId { get; set; }

        public DateTime ResponseTime { get; set; }

        public object Response { get; set; }
    }
}
