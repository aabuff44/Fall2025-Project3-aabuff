using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_aabuff.Models
{
    public class Actor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

        [Url]
        public string IMDBLink  { get; set; }
        //making Photo optional
        public byte[]? Photo { get; set; }

        public ICollection<Actor_Movie>? Actor_Movies { get; set; }
    }
}
