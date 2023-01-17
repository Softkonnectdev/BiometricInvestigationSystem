using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UBA_Network_Security_System.Models
{
    public class Log : BaseEntity
    {
        [Required(ErrorMessage ="Event is NULL")]
        public string Event { get; set; }
        [Required(ErrorMessage ="Initiator is NULL")]
        public string Initiator { get; set; }

    }
}