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
using test.Models;
using test.DAO;
using test.Controllers;

namespace test.Pages
{
    public class BookingModel : PageModel
    {
        public BookingPageViewModel BookingPageViewModel;
        public int Id = -1;

        //Страница бронирования мест
        //Id - это id сеанса(Schedule)
        private readonly AuthService _authService;
        public BookingModel(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult OnGet(int id)
        {
            Id = id;
            //Не авторизован - на главную станицу
            if (!_authService.IsAuthenticated)
                return Redirect(Url.Content("~/"));

            var schedule = FilmViewModelDAO.GetSchedule($"id_schedule = {id}").FirstOrDefault();
            schedule.Hall = FilmViewModelDAO.GetHallByScheduleId(id);

            var bookings = DAOFactory.GetBooknig(id);
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

            var schedule = FilmViewModelDAO.GetSchedule($"id_schedule = {id}").FirstOrDefault();
            var seats = FilmViewModelDAO.GetSeatsRowCountByHallId(schedule.Hall.HallNumber, schedule.Hall.RowCount);
            var bookedSeats = new List<Tuple<int, int, int>>();
            //Кэшируем места, которые хочет забронировать пользователь и проверяем
            //Не занято ли оно уже
            foreach (var index in result)
            {
                int row = index["row"];
                int seat = index["seat"];
               /* var curIndex = index;
                for (var i = 1; i < seats.Length - 1; i++)
                {
                    curIndex -= seats[i];
                    if (curIndex - seats[i + 1] <= 0)
                    {
                        row = i + 1;
                        seat = curIndex;
                    }
                }*/
                if (DAOFactory.Contains(string.Format("SELECT row = {0} AND place = {1} AND id_schedule = {2} FROM tickets", row, seat, id)))
                    return BadRequest(); //Status code 400
                var price = DAOFactory.FindPrice(string.Format("SELECT fix_price FROM schedule WHERE id_schedule = {0}", id));
                if (row == 1)
                    price /= 2;
                if (row == seats.Length - 1)
                    price = (int)Math.Ceiling(price * 1.2);
                bookedSeats.Add(new Tuple<int, int, int>(row, seat, price));
            }
            foreach (var pair in bookedSeats)
            {
                DAOFactory.AddData(string.Format("INSERT INTO tickets(row, place, id_schedule, price, id_user, code) VALUES " +
                    "({0}, {1},{2},{3},{4},{5})", pair.Item1, pair.Item2, id, pair.Item3, _authService.Id, GenerateCode()));
            }
            return Redirect(Url.Content($"~/booking/{id}"));
        }

        private static Tuple<int, int> GetPlace(string index)
        {
            var a = index;
            return Tuple.Create(0, 0);
        }

        private static string GenerateCode()
        {
            var code = new Random().Next(100000000, 999999999);
            return code.ToString();
        }
    }
}
