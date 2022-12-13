using System.ComponentModel.DataAnnotations;

namespace Zawody.Validation
{
    public class CustomHireDate : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            return dateTime <= DateTime.Now;
        }
    }
}
