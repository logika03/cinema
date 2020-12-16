using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test.Models;
using test.DAO;
using System.Diagnostics;

namespace test.Pages
{
    public class FilmsModel : PageModel
    {
        public FilmsViewModel FilmsViewModel;
        public BookingViewModel BookingViewModel;
        public IActionResult OnGet(int page = 1)
        {
            CreateFilmsList(page);
            return Page();
        }
        public IActionResult OnPost(int page = 1)
        {
            CreateFilmsList(page);
            return Page();
        }

        private void CreateFilmsList(int page)
        {
            var films = MainViewModelDAO.GetFilms(""); // FilmViewModelDAO.GetFilms("WHERE is_rent = 1", false);
            var filmsPerPage = 10;
            var totalPages = films.Count / filmsPerPage + (films.Count % filmsPerPage > 0 ? 1 : 0);
            FilmsViewModel = new FilmsViewModel(
                films.Skip(filmsPerPage * (page - 1)).Take(filmsPerPage),
                page,
                totalPages);
        }
    }
}
