using System.ComponentModel.DataAnnotations.Schema;

namespace Zawody.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public byte[]? Data { get; set; }
        public bool IsSelected { get; set; } = false;
        [NotMapped]
        public IFormFile? MyImage { set; get; }
    }
}
