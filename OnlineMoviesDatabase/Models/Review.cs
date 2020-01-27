using System;
namespace OnlineMovieDatabase.Models
{
    public class Review
    {
        public Review()
        {

        }
        public Review(int _movieId, int _userId, double _rating, string _reviewText)
        {
            UserId = _userId;
            MovieId = _movieId;
            ReviewText = _reviewText;
            Rating = _rating;
            ReviewDate = DateTime.Now;
        }
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public double Rating { get; set; }
    }
}