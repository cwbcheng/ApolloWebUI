using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class RobotLogModel
    {
        public long Time { get; set; }

        public string RepositoryName { get; set; }

        public int ProcessId { get; set; }

        public string Message { get; set; }

        public string Level { get; set; }

        public string FormId { get; set; }

        public string mac { get; set; }
    }
}
