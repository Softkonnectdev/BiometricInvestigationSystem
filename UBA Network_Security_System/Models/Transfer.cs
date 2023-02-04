using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UBA_Network_Security_System.Models
{
    public class Transfer : BaseEntity
    {
        [Required(ErrorMessage = "Account Number is NULL")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Account Number")]
        [StringLength(10, ErrorMessage = "Not more than 10 characters!")]
        [Display(Name = "Account Number")]
        public string SenderAccountNumber
        {
            get; set;
        }

        [Required(ErrorMessage = "Reciever Account Number is NULL")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Account Number")]
        [StringLength(10, ErrorMessage = "Not more than 10 characters!")]
        [Display(Name = "Reciever Account Number")]
        public string RecieverAccountNumber
        {
            get; set;
        }

        [Required(ErrorMessage = "Reciever Name is NULL!")]
        [Display(Name = "Reciever Name")]
        public string RecieverAccountName
        {
            get; set;
        }
       
        [Required(ErrorMessage = "Sender Phone is NULL!")]
        [Display(Name = "Phone Number")]
        public string SenderPhone
        {
            get; set;
        }
        [Required(ErrorMessage = "Status is NULL!")]
        public bool Status
        {
            get; set;
        }
        [Required(ErrorMessage = "Remark is NULL!")]
        [Display(Name = "Account Number")]
        public string Remark
        {
            get; set;
        }
        [Required(ErrorMessage = "Secure Pass is NULL")]
        [DataType(DataType.Password, ErrorMessage = "Invalid Secure Pass")]
        [StringLength(6, ErrorMessage = "Not more than 6 characters!")]
        public string SecurePass
        {
            get; set;
        }
        [Required(ErrorMessage = "Amount is NULL")]
        [DataType(DataType.Currency, ErrorMessage = "Not valid currency value")]
        public decimal Amount
        {
            get; set;
        }

        [Required(ErrorMessage = "Account ID is NULL")]
        [ForeignKey("Account")]
        public string AccountID
        {
            get; set;
        }

        [Required(ErrorMessage = "Cashier is NULL!")]
        [ForeignKey("User")]
        public string CashaierID
        {
            get; set;
        }

        public virtual Account Account { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}