using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class AlarmCondition
    {
        public AlarmConditionType ConditionType { get; set; }

        public string ProductId { get; set; }

    }

    public enum AlarmConditionType
    {

    }
}
