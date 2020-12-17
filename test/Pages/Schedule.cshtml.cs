using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using test.Models;
using test.DAO;

namespace test.Pages
{
    public class ScheduleModel : PageModel
    {
        public IEnumerable<FilmViewModel> Films;
        public IActionResult OnGet()
        {
            CreateListFilmsByDate();
            return Page();
        }

        public IActionResult OnPost()
        {
            CreateListFilmsByDate();   
            return Page();
        }

        private void CreateListFilmsByDate()
        {
            //Тоже самое что и на странице фильмов с расписанием
            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day);
            if (Request.Query.ContainsKey("schedule_date"))
                date = DateTime.ParseExact(
                    Request.Query["schedule_date"],
                    "dd.MM.yyyy", CultureInfo.InvariantCulture);
            HttpContext.Items["CurrentDay"] = date;
            
            var scheduleCurrent = DAOFactory.GetIdFilmWithSchedule(date);
            //Передаем во View только те фильмы, которые имеют хотя бы один сеанс на выбранную дату date
            Films = FilmViewModelDAO.GetFilms(
                string.Format("WHERE id IN (SELECT id_movie FROM schedule WHERE date::DATE = '{0}')", date.ToString("yyyy-MM-dd")), false);
            foreach (var film in Films)
                film.Schedule = scheduleCurrent[film.Id];
        }
    }
}
