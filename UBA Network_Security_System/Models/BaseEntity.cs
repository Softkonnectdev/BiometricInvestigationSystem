using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UBA_Network_Security_System.Models
{
    public abstract class BaseEntity
    {
        [Key] 
        public string Id { get; set; }
        [Display(Name = "Creaeted Date")]
        public string CreatedAt { get; set; }

        public BaseEntity()
        {
            var dateTime = DateTime.Now.ToString();
            //var dateTime = DateTime.Now.ToString().Replace("/", "");
            //dateTime.Replace(":", "");

            this.Id = Guid.NewGuid().ToString();
            //this.CreatedAt = DateTime.Now.ToString("d");
            this.CreatedAt = dateTime;
        }
    }
}
