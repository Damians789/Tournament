using System.ComponentModel.DataAnnotations;
using Zawody.Validation;

namespace Zawody.Models
{
    public class Trener : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        [CustomHireDate(ErrorMessage = "Hire Date must be less than or equal to Today's Date")]
        public DateTime? HireDate { get; set; }

    }
}
