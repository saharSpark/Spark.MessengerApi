using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spark.MessengerApi.Models
{
    public class NotificationTransactionFeedbackDTO
    {
        public string LocalId { get; set; }

        public string ExternalId { get; set; }

        public DateTimeOffset At { get; set; }

        public bool ReinitSession { get; set; }

        public string FileId { get; set; }

        public bool IsScrambled { get; set; }
    }
}