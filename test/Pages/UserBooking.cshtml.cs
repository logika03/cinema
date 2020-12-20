using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using cinema.Models;
using cinema.DAO;
using System;

namespace cinema.Pages
{
    public class UserBookingModel : PageModel
    {
        public UserViewModel UserViewModel;

        public IActionResult OnGet(int id)
        {
            UserViewModel = UserDAO.GetUserById(id);
            UserViewModel.Bookings = BookingDAO.GetBookingsByUserId(id);
            foreach (var booking in UserViewModel.Bookings)
            {
                booking.Schedule = ScheduleDAO.GetSchedule($"id_schedule = {booking.ScheduleId}").FirstOrDefault();
                booking.Schedule.Hall = ScheduleDAO.GetHallByScheduleId(booking.Schedule.Id);
                booking.Schedule.Film = FilmViewModelDAO.GetFilms($"WHERE id = {booking.Schedule.FilmId}", false).FirstOrDefault();
                if (booking.Row == 0)
                    booking.Schedule.PricePerSeat /= 2;
                else if (booking.Row == booking.Schedule.Hall.SeatsRowCount.Length - 1)
                    booking.Schedule.PricePerSeat = Math.Round(booking.Schedule.PricePerSeat * (decimal) 1.2);
            }
            return Page();
        }
    }
}
