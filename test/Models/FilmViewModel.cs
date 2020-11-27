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
        public int Rating;
        public List<string> Genres;
        public string ImagePath;
        public TimeSpan Duration;

        public string Producer;
        public IEnumerable<string> Actors;

        public IEnumerable<ScheduleViewModel> Schedule;
        public IEnumerable<ReviewViewModel> Reviews;
    }
}