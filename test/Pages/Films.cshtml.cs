using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using cinema.Models;
using cinema.DAO;
using System.Diagnostics;
using System.Text;
using Npgsql;

namespace cinema.Pages
{
    public class FilmsModel : PageModel
    {
        public FilmsViewModel FilmsViewModel;
        public IActionResult OnGet(string search, string genre_filter, string sortby, int page = 1)
        {
            CreateFilmsList(page, genre_filter, search, sortby);
            ViewData["SortBy"] = sortby;
            ViewData["SearchString"] = search;
            ViewData["GenresFilter"] = genre_filter;
            return Page();
        }
        public IActionResult OnPost(string search, string genre_filter, string sortby, int page = 1)
        {
            CreateFilmsList(page, genre_filter, search, sortby);
            return Page();
        }

        private void CreateFilmsList(int page, string genre_filter, string search, string sortby)
        {
            var whereStatement = new StringBuilder("WHERE is_rent = 1");
            var parameters = new List<NpgsqlParameter>();

            if (!string.IsNullOrWhiteSpace(search))
            {
                whereStatement.Append(" AND LOWER(title) LIKE '%' || @title || '%'");
                parameters.Add(new NpgsqlParameter("title", search.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(sortby))
            {
                whereStatement.Append(" ORDER BY");
                switch (sortby)
                {
                    case "date": whereStatement.Append(" release_year");
                        break;
                    
                    case "rating": whereStatement.Append(" raiting");
                        break;
                }

                whereStatement.Append(" DESC");
            }

            var films = FilmViewModelDAO.GetFilms(whereStatement.ToString(), false, parameters.ToArray());
            
            if (!string.IsNullOrWhiteSpace(genre_filter))
            {
                var genres = genre_filter.Split(',')
                    .Select(genre => genre.Trim().ToLower())
                    .ToHashSet();

                films = films
                    .Where(film => film.Genres
                        .Any(genre => genres.Contains(genre)))
                    .ToList();
            }

            var filmsPerPage = 10;
            var totalPages = films.Count / filmsPerPage + (films.Count % filmsPerPage > 0 ? 1 : 0);
            FilmsViewModel = new FilmsViewModel(
                films.Skip(filmsPerPage * (page - 1)).Take(filmsPerPage),
                page,
                totalPages);
        }
    }
}
