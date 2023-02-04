using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UBA_Network_Security_System.Models.Utility;

namespace UBA_Network_Security_System.Models
{
    public class BVNBank
    {
        [Key]
        [StringLength(11, ErrorMessage ="Not more than 11 characters!")]
        public string BVN { get; set; }

        [Required(ErrorMessage = "Sur Name is NULL")]
        [Display(Name = "Sur Name")]
        public string SurName
        {
            get; set;
        }

        [Required(ErrorMessage = "Sur Name is NULL")]
        [Display(Name = "Sur Name")]
        public string Gender
        {
            get; set;
        }

        [Required(ErrorMessage = "First Name is NULL")]
        [Display(Name = "First Name")]
        public string FirstName
        {
            get; set;
        }

        [Required(ErrorMessage = "Middle Name is NULL")]
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
        [Required(ErrorMessage = "Phone is NULL")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [StringLength(11, ErrorMessage = "Phone Not more than 11 characters!")]
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

        public DateTime CreatedOn { get; set; }


        public virtual ICollection<Account> Accounts { get; set; }

        public BVNBank()
        {
            var util = new Utilities();

            this.CreatedOn = DateTime.Now;
            this.BVN = util.RandomDigits(11);
        }
    }
}