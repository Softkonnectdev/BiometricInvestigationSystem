using System.ComponentModel.DataAnnotations;
using System.Web;

namespace CrimeInvestigationSystem.Models
{
    public class ComparePrintViewModel
    {
        [Required]
        [Display(Name = "Suspect Finger Print")]
        public HttpPostedFileBase SuspectPrint { get; set; }
        
    }
}
