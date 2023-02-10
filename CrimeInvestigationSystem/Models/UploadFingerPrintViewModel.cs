using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CrimeInvestigationSystem.Models
{
    public class UploadFingerPrintViewModel
    {

        [Required]
        [Display(Name = "User Left Thumb")]
        public HttpPostedFileBase LeftThumUpload { get; set; }

        [Required]
        [Display(Name = "User Right Thumb")]
        public HttpPostedFileBase RightThumUpload { get; set; }
         
        [Required(ErrorMessage ="Email is NULL")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address!")]
        public string Email { get; set; }

    }
}
