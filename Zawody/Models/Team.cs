using System.ComponentModel.DataAnnotations;

namespace Zawody.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Team's name")]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Tag")]
        public string ShortenedName { get; set; }
        public int? StadionID { get; set; }
        public int? TrenerID { get; set; }

        public Stadion? Stadion { get; set; }
        public Trener? Trener { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
