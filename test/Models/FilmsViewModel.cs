using System.Collections.Generic;

namespace cinema.Models
{
    public class FilmsViewModel
    {
        public IEnumerable<FilmViewModel> Films;
        public int CurrentPage;
        public int NumberOfPages;

        public FilmsViewModel(IEnumerable<FilmViewModel> films, int currentPage = 1, int numberOfPages = 1)
        {
            Films = films;
            CurrentPage = currentPage;
            NumberOfPages = numberOfPages;
        }
    }
}