using System.ComponentModel.DataAnnotations;

namespace UBA_Network_Security_System.Models
{
    public class TransactionLedger : BaseEntity
    {
        [Required(ErrorMessage ="Transaction is NULL")]
        public string TransactionId { get; set; }
        [Required(ErrorMessage ="Event is NULL")]
        public string TransactionType { get; set; } 

    }
}