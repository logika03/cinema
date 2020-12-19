using System;

namespace test.Models
{
    public class ReviewViewModel
    {
        public int UserId;
        public UserViewModel User;
        public string ReviewText;
        public DateTime TimeOfReview;
        public int Rating;

        public ReviewViewModel(UserViewModel user, string review, DateTime time, int rating)
        {
            User = user;
            ReviewText = review;
            TimeOfReview = time;
            Rating = rating;
        }

        public ReviewViewModel()
        { }
    }
}