using System.Collections;
using System.Collections.Generic;

namespace test.Models
{
    public class FilmsViewModel
    {
        public IEnumerable<FilmViewModel> Films;
        public int CurrentPage;
        public int NumberOfPages;
    }
}