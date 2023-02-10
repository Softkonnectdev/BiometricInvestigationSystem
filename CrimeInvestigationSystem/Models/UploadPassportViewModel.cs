using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CrimeInvestigationSystem.Models
{
    public class UploadPassportViewModel
    {
        [Required]
        [Display(Name = "User Passport")]
        public HttpPostedFileBase PassportUpload { get; set; }
        [Required(ErrorMessage ="Email is NULL")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }

    }
}
