namespace Zawody.Models
{
    public class Player : Person
    {
        public int? TeamID { get; set; }
        public string? Pozycja { get; set; }
    }
}
