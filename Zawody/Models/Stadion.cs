using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Zawody.Models
{
    public class Stadion
    {
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public int Pojemnosc { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }
        public ICollection<Team>? Teams { get; set; }
    }
}
