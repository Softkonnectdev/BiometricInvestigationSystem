using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using UBA_Network_Security_System.Models.Utility;

namespace UBA_Network_Security_System.Models
{
    public class Account
    {
        [Key]
        [Required(ErrorMessage = "Account Number is NULL")]
        [StringLength(10, ErrorMessage = "Not more than 10 characters!")]
        public string AccountNumber
        {
            get; set;
        }

        [Required(ErrorMessage = "Account Type is NULL")]
        [Display(Name = "Account Type")]
        public string AccountTypeID
        {
            get; set;
        }




        [Required(ErrorMessage = "Surname is NULL")]
        [Display(Name = "Sur Name")]
        public string SurName
        {
            get; set;
        }
        [Required(ErrorMessage = "FirstName is NULL")]
        [Display(Name = "First Name")]
        public string FirstName
        {
            get; set;
        }
        [Required(ErrorMessage = "MiddleName is NULL")]
        [Display(Name = "Middle Name")]
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

        public byte[] Passport
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
        [DisplayFormat(DataFormatString = "dd-MM-yyyy", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB
        {
            get; set;
        }

        [Required(ErrorMessage = "DOO is NULL")]
        [DisplayFormat(DataFormatString = "dd-MM-yyyy", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Opening")]
        public DateTime DOO
        {
            get; set;
        }
        [Required(ErrorMessage = "Gender is NULL")]
        public string Gender
        {
            get; set;
        }
        [Required(ErrorMessage = "Residential Address is NULL")]
        public string ResidentialAddress
        {
            get; set;
        }
        [Required(ErrorMessage = "Permanent Address is NULL")]
        [Display(Name = "Permanent Resident")]
        public string PermanentResident
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
        [Required(ErrorMessage = "Residential Country is NULL")]
        [Display(Name = "Residential Country")]
        public string ResidentialCountry
        {
            get; set;
        }
        [Required(ErrorMessage = "Nationality is NULL")]
        public string Nationality
        {
            get; set;
        }
        [Required(ErrorMessage = "BVN is NULL")]
        [ForeignKey("BVNBank")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid BVN")]
        [StringLength(11, ErrorMessage = "Not more than 11 characters!")]
        public string BVN
        {
            get; set;
        }
        [Required(ErrorMessage = "Branch Office is NULL")]
        [Display(Name = "Branch Office")]
        public string BranchOffice
        {
            get; set;
        }
        [Required(ErrorMessage = "Marital Status is NULL")]
        [Display(Name = "Marital Status")]
        public string MaritalStatus
        {
            get; set;
        }
        [Required(ErrorMessage = "Occupation is NULL")]
        public string Occupation
        {
            get; set;
        }
        [Display(Name = "Introduced By")]
        public string IntroducedBy
        {
            get; set;
        }

        [Required(ErrorMessage = "SecurePass is NULL")]
        [DataType(DataType.Password, ErrorMessage = "Invalid Secure Pass")]
        [StringLength(6, ErrorMessage = "Not more than 6 characters!")]
        public string SecurePass
        {
            get; set;
        }

        public byte[] Signature
        {
            get; set;
        }

        [Required(ErrorMessage = "IsActive is NULL")]
        public bool IsActive
        {
            get; set;
        }

        //CUSTOMER IDENTIFICATION
        [Required(ErrorMessage = "Identification Type is NULL")]
        [Display(Name = "Identification Type")]
        public string IdentificationType
        {
            get; set;
        }

        [Required(ErrorMessage = "Identification Number is NULL")]
        [Display(Name = "Identification Number")]
        public string IdentificationNumber
        {
            get; set;
        }

        [Required(ErrorMessage = "Identification-IssuedDate is NULL")]
        [Display(Name = "Issued On")]
        public int IdentificationIssuedDate
        {
            get; set;
        }
        [Required(ErrorMessage = "Identification-ValidTill is NULL")]
        [Display(Name = "Valid Till")]
        public int IdentificationValidTill
        {
            get; set;
        }

        //GENERAL ACCOUNT INFO
        [Required(ErrorMessage = "Balance is NULL")]
        [DataType(DataType.Currency, ErrorMessage = "Not valid currency value")]
        public decimal Balance
        {
            get; set;
        }

        [Display(Name = "Account Manager")]
        public string AccountManagerName
        {
            get; set;
        }





        public virtual BVNBank BVNBank { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
        public virtual ICollection<Deposit> Deposits { get; set; }
        public virtual ICollection<Withdrawal> Withdrawals { get; set; }




        public Account()
        {
            var dateTime = DateTime.Now;
            Utilities util = new Utilities();
            var _accNo = util.RandomDigits(10);

            this.AccountNumber = _accNo;

        }


    }
}