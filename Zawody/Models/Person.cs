using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zawody.Models
{
    public abstract class Person
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
        [ForeignKey("UserFK")]
        public string? CreatedById { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public int get_age
        {
            get{
                int age = 0;
                age = DateTime.Now.Subtract(DateOfBirth).Days;
                age = age / 365;
                return age;
            }
        }
        /*public virtual ZawodyUser Creator { get; set; }*/
        public Team? Team { get; set; }
    }
}
