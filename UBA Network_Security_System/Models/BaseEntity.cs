using System;
using System.ComponentModel.DataAnnotations;
using UBA_Network_Security_System.Models.Utility;

namespace UBA_Network_Security_System.Models
{
    public abstract class BaseEntity
    {
        [Key] 
        public string Id { get; set; }
        [Display(Name = "Creaeted Date")]
        public DateTime CreatedAt { get; set; }

        public BaseEntity()
        {
            //var dateTime = DateTime.Now.ToString();
            var dateTime = DateTime.Now;
            //dateTime.Replace(":", "");

            Utilities util = new Utilities();
            var _id = util.RandomDigits(15);

            //this.Id = Guid.NewGuid().ToString();
            this.Id = _id;
            //this.CreatedAt = DateTime.Now.ToString("d");
            this.CreatedAt = dateTime;
        }
    }
}
