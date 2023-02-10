using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CrimeInvestigationSystem.Models.Utility;

namespace CrimeInvestigationSystem.Models
{
    public class Profile
    {
        [Key]
        [Required(ErrorMessage = "Profile ID is NULL")]
        [StringLength(10, ErrorMessage = "Not more than 10 characters!")]
        public string ProfileId
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

        public string LeftThumb { get; set; }

        public string RightThumb { get; set; }



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

        [Required(ErrorMessage = "CreateOn is NULL")]
        [DisplayFormat(DataFormatString = "dd-MM-yyyy", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Opening")]
        public DateTime CreatedOn
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

        [Required(ErrorMessage = "Marital Status is NULL")]
        [Display(Name = "Marital Status")]
        public string MaritalStatus
        {
            get; set;
        }

        [Required(ErrorMessage = "Next of Kin Name is NULL")]
        [Display(Name = "Next of Kin")]
        public string NextofKinName
        {
            get; set;
        }

        [Required(ErrorMessage = "Next of Kin Phone is NULL")]
        [Display(Name = "Next of Kin Phone")]
        public string NextofKinPhone
        {
            get; set;
        }

        [Required(ErrorMessage = "Occupation is NULL")]
        public string Occupation
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

        public virtual ICollection<Crime> Crimes { get; set; }




        public Profile()
        {
            var dateTime = DateTime.Now;
            Utilities util = new Utilities();
            var id = util.RandomDigits(10);

            this.ProfileId = id;

        }


    }
}