using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.MessengerApi.Models
{
    class InternalModels
    { }
    public struct IdentityValidation
    {
        public int TypeId { get; set; }
        public string Identity { get; set; }
        public string Token { get; set; }
        public string Validation { get; set; }
        public bool Immediate { get; set; }
        public string Template { get; set; }

    }
    public struct GenericNotificationFeedback
    {
        public string Id { get; set; }
        public GroupDTO Group { get; set; }
        public List<RecipientDTO> Recipients { get; set; }
        public int? TypeId { get; set; }
    }

}
