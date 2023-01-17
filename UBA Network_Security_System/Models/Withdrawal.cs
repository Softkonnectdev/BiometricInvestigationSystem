using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UBA_Network_Security_System.Models
{
    public class Withdrawal : BaseEntity
    { 
        [Required(ErrorMessage = "Account Number is NULL!")]
        public string AccountNumber { get; set; }
        [Required(ErrorMessage = "Account Name is NULL!")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Status is NULL!")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Remark is NULL!")]
        public string Remark { get; set; }
        [Required(ErrorMessage = "SecurePass is NULL!")]
        public string SecurePass { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("Account")]
        public string AccountID { get; set; }
        public virtual Account Account { get; set; }
    }
}