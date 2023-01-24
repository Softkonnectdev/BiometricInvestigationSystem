using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UBA_Network_Security_System.Models
{
    public class Employee
    {
        [Key]
        [ForeignKey("User")]
        public string UserId
        {
            get; set;
        }
        [Required(ErrorMessage = "Surname is NULL")]
        public string SurName
        {
            get; set;
        }
        [Required(ErrorMessage = "FirstName is NULL")]
        public string FirstName
        {
            get; set;
        }
        [Required(ErrorMessage = "MiddleName is NULL")]
        public string MiddleName
        {
            get; set;
        }
        [Required(ErrorMessage = "Email is NULL")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email
        {
            get; set;
        }
        [Required(ErrorMessage = "Phone is NULL")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [StringLength(11, ErrorMessage = "Not more than 11 characters!")]
        public string Phone
        {
            get; set;
        }
        [Required(ErrorMessage = "DOB is NULL")]
        public DateTime DOB
        {
            get; set;
        }
        [Required(ErrorMessage = "DOE is NULL")]
        public DateTime DOE
        {
            get; set;
        }
        [Required(ErrorMessage = "Gender is NULL")]
        public string Gender
        {
            get; set;
        }
        [Required(ErrorMessage = "Referee is NULL")]
        public string Referee
        {
            get; set;
        }
        [Required(ErrorMessage = "RefereePhone is NULL")]
        public string RefereePhone
        {
            get; set;
        }

        [Required(ErrorMessage = "ResidentialAddress is NULL")]
        public string ResidentialAddress
        {
            get; set;
        }
        [Required(ErrorMessage = "LGA is NULL")]
        public string LGA
        {
            get; set;
        }
        [Required(ErrorMessage = "State is NULL")]
        public string State
        {
            get; set;
        }
        [Required(ErrorMessage = "ResidentialCountry is NULL")]
        public string ResidentialCountry
        {
            get; set;
        }
        [Required(ErrorMessage = "Nationality is NULL")]
        public string Nationality
        {
            get; set;
        }


        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
        public virtual ICollection<Withdrawal> Withdrawals { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
    }
}