using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test.Models;
using test.DAO;
using test.Controllers;

namespace test.Pages
{
    public class FilmModel : PageModel
    {
        public BookingPageViewModel BookingPageViewModel;
        public FilmViewModel FilmViewModel;
        public List<ScheduleViewModel> ScheduleCurrent;
        private readonly AuthService _authService;
        public FilmModel(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult OnGet(int id)
        {
            CreateDataAboutFilm(id);
            return Page();
        }
        public IActionResult OnPost(int id, string review, int rating)
        {
            CreateDataAboutFilm(id);
            if (_authService.IsAuthenticated)
            {
                var userId = _authService.Id;
                var sqlExpression = string.Format("INSERT INTO reviews(id_movie, author, text, raiting) VALUES({0}, {1}, '{2}', {3})",
                    id, userId, review, rating);
                DAOFactory.AddData(sqlExpression);
                FilmViewModel.Reviews = FilmViewModelDAO.GetReviewByFilmId(id);
            }
            return Page();
        }

        private void CreateDataAboutFilm(int id)
        {
            //Ёто нужно, чтобы смотреть расписание фильма на разные дни
            //»нформаци€ о выбранном дне сохран€етс€ в HttpContext.Items["CurrentDay"]
            //ѕо умолчанию равен дате сегодн€
            var timeNow = DateTime.Now;
            var currentDay = timeNow.Date; //new DateTime(timeNow.Year,timeNow.Month, timeNow.Day); 
            if (Request.Query.ContainsKey("schedule_date"))
                currentDay = DateTime.ParseExact(
                    Request.Query["schedule_date"],
                    "dd.MM.yyyy", CultureInfo.InvariantCulture);
            HttpContext.Items["CurrentDay"] = currentDay;

            var film = FilmViewModelDAO.GetFilms($"WHERE id = {id}", false).FirstOrDefault();
            //film.Schedule = FilmViewModelDAO.GetScheduleById(id, "id_movie");
            FilmViewModel = film;

            FilmViewModel.Actors = FilmViewModelDAO.GetActorsByFilmId(FilmViewModel.Id);
            FilmViewModel.Producers = FilmViewModelDAO.GetProducersByFilmId(FilmViewModel.Id);
            FilmViewModel.Country = FilmViewModelDAO.GetCounrtyById(FilmViewModel.CountryId);
            FilmViewModel.Reviews = FilmViewModelDAO.GetReviewByFilmId(FilmViewModel.Id);
            FilmViewModel.Schedule = FilmViewModelDAO.GetSchedule($"id_movie={id} AND date::DATE = '{currentDay:yyyy-MM-dd}'");
        }
    }
}
