using System;

namespace test.Models
{
    public class ScheduleViewModel
    {
        public int Id;
        public decimal PricePerSeat;
        public FilmViewModel Film;
        public DateTime Time;
        public HallViewModel Hall;
        public int IdFilm;

        /*public ScheduleViewModel(int id, decimal pricePrSeat, FilmViewModel film, DateTime time, HallViewModel hall)
        {
            Id = id;
            PricePerSeat = pricePrSeat;
            Film = film;
            Time = time;
            Hall = hall;
        }*/
    }
}