using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UBA_Network_Security_System.Models
{
    public class Deposit : BaseEntity
    {
        [Required(ErrorMessage = "Account Number is NULL!")]
        public string AccountNumber { get; set; }
        [Required(ErrorMessage = "Account Name is NULL!")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Amount is NULL!")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Status is NULL!")]
        public bool Status { get; set; }
        [Required(ErrorMessage = "Remark is NULL!")]
        public string Remark { get; set; }
        [Required(ErrorMessage = "DepositorName is NULL!")]
        public string DepositorName { get; set; }
        [Required(ErrorMessage = "DepositorPhone is NULL!")]
        public string DepositorPhone { get; set; }
        [Required(ErrorMessage = "Cashier is NULL!")]
        public string Cashaier { get; set; }


    }
}