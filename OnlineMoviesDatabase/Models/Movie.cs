using System;
using System.Collections.Generic;

namespace OnlineMovieDatabase.Models
{
    public class Movie
    {
        public Movie()
        {

        }
        public int Id { get; set; }
        public string OriginalTitle { get; set; }
        public string RuTitle { get; set; }
        public string OriginalDescription { get; set; }
        public string RuDescription { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseYear { get { return ReleaseDate.Year.ToString(); } }
        public Double Rating { get; set; }
        public double RoundedRating { get
            {
                return Math.Round(Rating, 2);
            } }
        public string Genres;
        public List<MovieGenres> GenresList;
    }
}