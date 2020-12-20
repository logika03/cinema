using System;

namespace cinema.Models
{
    public class ScheduleViewModel
    {
        public int Id;
        public decimal PricePerSeat;
        public FilmViewModel Film;
        public DateTime Time;
        public HallViewModel Hall;

        public int FilmId { get; set; }
    }
}