using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CrimeInvestigationSystem.Models
{
    public class Crime : BaseEntity
    {
        [Required(ErrorMessage = "Crime Type is NULL!")]
        [Display(Name = "Crime Type")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Tried is NULL!")]
        public bool Tried { get; set; }

        [Display(Name = "Address of Crime")]
        [Required(ErrorMessage = "Address of Crime is NULL!")]
        public string AddressofCrime { get; set; }

        [Display(Name = "Defaulted Law")]
        [Required(ErrorMessage = "Defaulted is NULL!")]
        public string DefaultedLaw { get; set; }

        [Required(ErrorMessage = "Committed Date is NULL")]
        [DisplayFormat(DataFormatString = "dd-MM-yyyy", ApplyFormatInEditMode = true)]
        [Display(Name = "Crime Committed On")]
        [DataType(DataType.Date)]
        public DateTime CommittedDate
        {
            get; set;
        }
  
        public byte[] Photo { get; set; }

        [NotMapped]
        [Display(Name = "Crime Photo")]
        public HttpPostedFileBase PhotoUpload { get; set; }



        [Required(ErrorMessage = "Profile ID is NULL!")]
        [ForeignKey("Profile")]
        public string ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
}