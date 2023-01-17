
using System.ComponentModel.DataAnnotations;

namespace UBA_Network_Security_System.Models
{
    public class AccountStatement :BaseEntity
    {
        [Required(ErrorMessage = "Transaction Type is NULL")]
        public string TransactionType { get; set; }
        [Required(ErrorMessage = "Amount is NULL")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Status is NULL")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Remark is NULL")]
        public string Remark { get; set; }
        [Required(ErrorMessage = "TransactionId is NULL")]
        public string TransactionId { get; set; }
    }
}