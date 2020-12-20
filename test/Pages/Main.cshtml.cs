using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using cinema.Models;
using cinema.DAO;

namespace cinema.Pages.ProjectArt
{
    public class MainPageModel : PageModel
    {
        public MainViewModel MainModel;
        public IActionResult OnGet()
        {
            CreateDataMainModel();
            return Page();
        }
        public IActionResult OnPost()
        {
            CreateDataMainModel();
            return Page();
        }
        private void CreateDataMainModel()
        {

            MainModel = new MainViewModel
            {
                //В TopFilms Берем N лучших по рейтингу фильмов
                TopFilms = FilmViewModelDAO.GetFilms("WHERE is_rent = 1 ORDER BY raiting DESC LIMIT 6", true),
                //В TodayFilms берем фильмы, у которых есть хотя бы 1 сеанс сегодня
                TodayFilms = FilmViewModelDAO.GetFilms("WHERE is_rent = 1 AND EXISTS (SELECT 1 FROM schedule WHERE id_movie = id " +
                "AND(date::DATE = CURRENT_TIMESTAMP::DATE))", false)
            };

            var schedule = DAOFactory.GetIdFilmWithSchedule(DateTime.Now);
            
            foreach(var film in MainModel.TodayFilms)
            {
                film.Schedule = schedule[film.Id];
            };
        }
    }
}
