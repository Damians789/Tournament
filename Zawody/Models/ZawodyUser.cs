using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Zawody.Models
{
    public class ZawodyUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;

        public byte[]? ProfilePicture { get; set; }
    }
}
