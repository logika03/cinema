using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using cinema.Models;
using cinema.DAO;

namespace cinema.Pages
{
    public class BookingModel : PageModel
    {
        public BookingPageViewModel BookingPageViewModel;
        public int Id = -1;

        private readonly AuthService _authService;
        public BookingModel(AuthService authService)
        {
            _authService = authService;
        }

        //Страница бронирования мест
        //Id - это id сеанса(Schedule)
        public IActionResult OnGet(int id)
        {
            Id = id;
            //Не авторизован - на главную станицу
            if (!_authService.IsAuthenticated)
                return Redirect(Url.Content("~/"));
            var schedule = ScheduleDAO.GetSchedule($"id_schedule = {id}").FirstOrDefault();
            schedule.Hall = ScheduleDAO.GetHallByScheduleId(id);

            var bookings = BookingDAO.GetBookingByScheduleId(id);
            foreach (var booking in bookings)
                booking.Schedule = schedule;

            BookingPageViewModel = new BookingPageViewModel
            {
                //Передаем сеанс с указаным id
                Schedule = schedule,
                //Передаем все бронирования на этот сеанс
                BookingsInSchedule = bookings
            };
            return Page();
        }


        public async Task<IActionResult> OnPost(int id)
        {
            if (!_authService.IsAuthenticated)
                return BadRequest(); //Status code 400

            //Данные приходят как массив индексов мест в формате json
            var json = "";
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                json = await reader.ReadToEndAsync();
            }

            dynamic result = JsonConvert.DeserializeObject(json);

            var bookedSeats = new List<Tuple<int, int>>();
            //Кэшируем места, которые хочет забронировать пользователь и проверяем
            //Не занято ли оно уже
            foreach (var index in result)
            {
                int row = index["row"];
                int seat = index["seat"];
                if (DAOFactory.Contains(string.Format("SELECT row = {0} AND place = {1} AND id_schedule = {2} FROM tickets", row, seat, id)))
                    return BadRequest(); //Status code 400
                bookedSeats.Add(new Tuple<int, int>(row, seat));
            }
            foreach (var pair in bookedSeats)
            {
                DAOFactory.AddData(string.Format("INSERT INTO tickets(row, place, id_schedule, id_user, code) VALUES " +
                    "({0}, {1},{2},{3},{4})", pair.Item1, pair.Item2, id, _authService.Id, GenerateCode()));
            }
            return new OkResult();
        }

        private static string GenerateCode()
        {
            var code = new Random().Next(100000000, 999999999);
            return code.ToString();
        }
    }
}
