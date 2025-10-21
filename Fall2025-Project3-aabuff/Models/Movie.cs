using System.ComponentModel.DataAnnotations;

namespace Fall2025_Project3_aabuff.Models
{
    public class Movie
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        [Url]
        public string IMDBLink { get; set; }

        public string Genre { get; set; }
        public int Year { get; set; }
        //Making poster optional
        public byte[]? Poster { get; set; }

        public ICollection<Actor_Movie>? Actor_Movies { get; set; }


    }
}
