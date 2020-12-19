using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test.Models;
using test.DAO;
using System.Diagnostics;

namespace test.Pages.ProjectArt
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
                //� TopFilms ����� N ������ �� �������� �������
                TopFilms = FilmViewModelDAO.GetFilms("WHERE is_rent = 1 ORDER BY raiting DESC LIMIT 6", true),
                //� TodayFilms ����� ������, � ������� ���� ���� �� 1 ����� �������
                TodayFilms = FilmViewModelDAO.GetFilms("WHERE is_rent = 1 AND EXISTS (SELECT * FROM schedule WHERE id_movie = id " +
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
