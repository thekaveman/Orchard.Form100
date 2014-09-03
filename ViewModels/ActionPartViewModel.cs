using System.ComponentModel.DataAnnotations;
using CSM.Form100.Models;

namespace CSM.Form100.ViewModels
{
    public class ActionPartViewModel
    {
        [Required]
        public ActionCategory Category { get; set; }
                
        [Required]
        public string Type { get; set; }

        public string Detail { get; set; }

        [Required,
         Display(Name = "Effective Date")]
        public string EffectiveDate { get; set; }
    }
}