using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using test.Models;
using test.DAO;

namespace test.Pages
{
    public class UserBookingModel : PageModel
    {
        public UserViewModel UserViewModel;

        public IActionResult OnGet(int id)
        {
            UserViewModel = FilmViewModelDAO.GetUserById(id);
            UserViewModel.Bookings = FilmViewModelDAO.GetBookingsByUserId(id);
            foreach (var booking in UserViewModel.Bookings)
            {
                booking.Schedule = FilmViewModelDAO.GetSchedule($"id_schedule = {booking.ScheduleId}").FirstOrDefault();
                booking.Schedule.Hall = FilmViewModelDAO.GetHallByScheduleId(booking.Schedule.Id);
                booking.Schedule.Film = FilmViewModelDAO.GetFilms($"WHERE id = {booking.Schedule.FilmId}", false).FirstOrDefault();
            }
            return Page();
        }
    }
}
