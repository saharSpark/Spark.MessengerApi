using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spark.MessengerApi.Models
{
    public class UserInfoDTO
    {
        public string Name { get; set; }
        public string CountryId { get; set; }        
        public DateTimeOffset? UpdatedAt {get; set;}
    }
}