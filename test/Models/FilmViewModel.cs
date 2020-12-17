using System;
using System.Collections;
using System.Collections.Generic;

namespace test.Models
{
    public class FilmViewModel
    {
        public int Id;
        public string Name;
        public string Description;
        public int Year;
        public double Rating;
        public List<string> Genres;
        public string ImagePath;
        public int DurationInMinutes;
        public string Country;

        public IEnumerable<string> Producers;
        public IEnumerable<string> Actors;

        public IEnumerable<ScheduleViewModel> Schedule;
        public IEnumerable<ReviewViewModel> Reviews;

        /*public FilmViewModel(int id, string name, string description, int year,
        int rating, List<string> genres, string imagePath, TimeSpan duration,
        List<string> producers, IEnumerable<string> actors, IEnumerable<ScheduleViewModel> schedule,
        IEnumerable<ReviewViewModel> reviews)
        {
            Id = id;
            Name = name;
            Description = description;
            Year = year;
            Rating = rating;
            Genres = genres;
            ImagePath = imagePath;
            Duration = duration;
            Producers = producers;
            Actors = actors;
            Schedule = schedule;
            Reviews = reviews;
        }*/
        public int CountryId { get; set; }
    }
}