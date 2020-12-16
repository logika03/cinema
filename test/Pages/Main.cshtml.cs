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
        public BookingPageViewModel ViewModel;
        public MainViewModel MainModel;
        public Dictionary<int, List<ScheduleViewModel>> TodaySchedule;

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
            TodaySchedule = DAOFactory.GetIdFilmWithSchedule(DateTime.Now);
        }
    }
}
