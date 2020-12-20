using System.Collections;
using System.Collections.Generic;

namespace cinema.Models
{
    public class MainViewModel
    {
        public IEnumerable<FilmViewModel> TopFilms;
        public IEnumerable<FilmViewModel> TodayFilms;
    }
}