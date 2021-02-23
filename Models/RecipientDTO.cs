using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spark.MessengerApi.Models
{
    public class RecipientDTO
    {
        public string Id { get; set; }
        public long UserId { get; set; }
        public bool? IsGroup { get; set; }
    }
}