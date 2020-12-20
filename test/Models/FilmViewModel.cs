using System.Collections.Generic;

namespace cinema.Models
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

        public int CountryId { get; set; }
    }
}