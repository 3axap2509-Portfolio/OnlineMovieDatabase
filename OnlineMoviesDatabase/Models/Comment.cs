using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineMovieDatabase.Models
{
    public class Comment
    {
        public Comment()
        {

        }
        public Comment(int _movieId, int _userId, string _commentText)
        {
            UserId = _userId;
            MovieId = _movieId;
            CommentText = _commentText;
            CommentDate = DateTime.Now;
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
