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
                TopFilms = MainViewModelDAO.GetFilms("ORDER BY raiting DESC LIMIT 6"),
                TodayFilms = MainViewModelDAO.GetFilms("AND EXISTS (SELECT 1 FROM schedule WHERE id_movie = id " +
                "AND(date::DATE = CURRENT_TIMESTAMP::DATE))")

            };
            TodaySchedule = DAOFactory.GetIdFilmWithSchedule(DateTime.Now);
        }
    }
}
