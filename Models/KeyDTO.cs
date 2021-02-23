using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spark.MessengerApi.Models.DTO
{
    public class KeyDTO
    {
        public string Key { get; set; }
        public int ExpiresIn { get; set; }
    }
}