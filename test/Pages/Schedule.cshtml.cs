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
        public Dictionary<int, List<ScheduleViewModel>> ScheduleCurrent;
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
            //���� ����� ��� � �� �������� ������� � �����������
            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day);
            if (Request.Query.ContainsKey("schedule_date"))
                date = DateTime.ParseExact(
                    Request.Query["schedule_date"],
                    "dd.MM.yyyy", CultureInfo.InvariantCulture);
            HttpContext.Items["CurrentDay"] = date;
            ScheduleCurrent = DAOFactory.GetIdFilmWithSchedule(date);
            //�������� �� View ������ �� ������, ������� ����� ���� �� ���� ����� �� ��������� ���� date
            Films = MainViewModelDAO.GetFilms(
                $"AND id IN (SELECT id_movie FROM schedule WHERE date::DATE = '{date:yyyy-MM-dd}')");
            /*foreach (var film in Films)
                film.Schedule = FilmViewModelDAO.GetScheduleById(film.Id, "id_movie");*/
        }
    }
}
