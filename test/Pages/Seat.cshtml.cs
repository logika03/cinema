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
using test.DAO;
using test.Models;

namespace test.Pages
{
    public class SeatModel : PageModel
    {
        private AuthService _authService;
        public SeatModel(AuthService authService)
        {
            _authService = authService;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            /*if (!_authService.IsAuthenticated)
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
            foreach (int index in result)
            {
                int row = 0;
                int seat = 0;
                var curIndex = index;
                for (var i = 1; i < seats.Length - 1; i++)
                {
                    curIndex -= seats[i];
                    if (curIndex - seats[i + 1] <= 0)
                    {
                        row = i + 1;
                        seat = curIndex;
                    }
                }
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
            }*/
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
            foreach (int index in result)
            {
                int row = 0;
                int seat = 0;
                var curIndex = index;
                for (var i = 1; i < seats.Length - 1; i++)
                {
                    curIndex -= seats[i];
                    if (curIndex - seats[i + 1] <= 0)
                    {
                        row = i + 1;
                        seat = curIndex;
                    }
                }
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
            return StatusCode(200); //Redirect(Url.Content($"~/booking/{id}"));
        }

        private static string GenerateCode()
        {
            var code = new Random().Next(100000000, 999999999);
            return code.ToString();
        }
    }
    
}
