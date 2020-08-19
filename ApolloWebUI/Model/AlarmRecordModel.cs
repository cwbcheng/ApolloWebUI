using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class AlarmRecordModel
    {
        public int Id { get; set; }

        [DefaultValue(null)]
        [MaxLength(20)]
        public string ProductId { get; set; }

        public string Message { get; set; }

        public string TraceId { get; set; }

        public int ErrorCode { get; set; }

        public DateTime Time { get; set; }
    }
}
