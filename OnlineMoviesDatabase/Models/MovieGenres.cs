namespace OnlineMovieDatabase.Models
{
    public class MovieGenres
    {
        public MovieGenres()
        {

        }
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int GenreId { get; set; }
    }
}