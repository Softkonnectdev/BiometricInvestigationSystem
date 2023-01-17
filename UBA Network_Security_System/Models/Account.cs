using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UBA_Network_Security_System.Models
{
    public class Account : BaseEntity
    {
        //CUSTOMER PROFILE
        [Required(ErrorMessage ="UserId is NULL")]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "BranchOffice is NULL")]
        public string AccountNumber { get; set; }
        [Required(ErrorMessage = "Account Type is NULL!")]
        public string AccountTypeID { get; set; }
        [Required(ErrorMessage = "Surname is NULL")]
        public string SurName { get; set; }
        [Required(ErrorMessage = "FirstName is NULL")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "MiddleName is NULL")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Email is NULL")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone is NULL")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "DOB is NULL")]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "DOO is NULL")]
        public DateTime DOO { get; set; }
        [Required(ErrorMessage = "Gender is NULL")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "ResidentialAddress is NULL")]
        public string ResidentialAddress { get; set; }
        [Required(ErrorMessage = "LGA is NULL")]
        public string LGA { get; set; }
        [Required(ErrorMessage = "State is NULL")]
        public string State { get; set; }
        [Required(ErrorMessage = "ResidentialCountry is NULL")]
        public string ResidentialCountry { get; set; }
        [Required(ErrorMessage = "Nationality is NULL")]
        public string Nationality { get; set; }
        [Required(ErrorMessage = "BVN is NULL")]
        public string BVN { get; set; }
        [Required(ErrorMessage = "BranchOffice is NULL")]
        public string BranchOffice { get; set; }
        [Required(ErrorMessage = "BranchOffice is NULL")]
        public string MaritalStatus { get; set; }
        [Required(ErrorMessage = "Occupation is NULL")]
        public string Occupation { get; set; }
         public string IntroducedBy { get; set; }

        [Required(ErrorMessage = "SecurePass is NULL")]
        public string SecurePass { get; set; }
        [Required(ErrorMessage = "Signature is NULL")]
        public byte[] Signature { get; set; }
        [Required(ErrorMessage = "Passport is NULL")]
        public byte[] Passport { get; set; }

        [Required(ErrorMessage = "IsActive is NULL")]
        public bool IsActive { get; set; }

        //CUSTOMER IDENTIFICATION
        [Required(ErrorMessage = "IdentificationType is NULL")]
        public string IdentificationType { get; set; }

        [Required(ErrorMessage = "IdentificationNumber is NULL")]
        public string IdentificationNumber { get; set; }

        [Required(ErrorMessage = "Identification-IssuedDate is NULL")]
        public string IdentificationIssuedDate { get; set; }
        [Required(ErrorMessage = "Identification-ValidTill is NULL")]
        public string IdentificationValidTill { get; set; }

        //GENERAL ACCOUNT INFO
        [Required(ErrorMessage = "Balance is NULL")]
        public decimal Balance { get; set; }
        [Required(ErrorMessage = "BranchOffice is NULL")]
        public string AccountManagerName { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
        public virtual ICollection<Withdrawal> Withdrawals { get; set; } 
    }
}