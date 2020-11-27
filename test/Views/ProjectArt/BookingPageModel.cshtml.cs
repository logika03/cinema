using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using test.Models;

namespace test.Views.ProjectArt
{
    public class BookingPageModel : PageModel
    {
        public BookingPageViewModel ViewModel;
        
        //Страница бронирования мест
        //Id - это id сеанса(Schedule)
        public IActionResult OnGet(int id)
        {
            //Не авторизован - на главную станицу
            if (!HttpContext.User.Identity.IsAuthenticated)
                return Redirect(Url.Content("~/"));
            
            var model = new BookingPageViewModel();
            //Передаем сеанс с указаным id
            model.Schedule = TestDataProvider.Schedules[id];
            //Передаем все бронирования на этот сеанс
            model.BookingsInSchedule = TestDataProvider.Bookings.Values
                .Where(booking => booking.Schedule.Id == id);
            ViewModel = model;
            return Page();
        }
        
        //Action на ajax запрос бронирования мест
        //id - Id сеанса
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
                return BadRequest(); //Status code 400

            //Данные приходят как массив индексов мест в формате json
            var json = "";
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                json = await reader.ReadToEndAsync();
            }

            dynamic result = JsonConvert.DeserializeObject(json);

            var schedule = TestDataProvider.Schedules[id];
            //Создаем хэш-таблицу(словарь) уже забронированных мест на этот сеанс
            var unavailableSeats = TestDataProvider.Bookings.Values
                    .Where(booking => booking.Schedule.Id == id)
                    .ToDictionary(booking => booking.Seat);

            var bookedSeats = new List<int>();
            
            //Кэшируем места, которые хочет забронировать пользователь и проверяем
            //Не занято ли оно уже
            foreach (int index in result)
            {
                if (unavailableSeats.ContainsKey(index))
                    return BadRequest(); //Status code 400

                bookedSeats.Add(index);
            }

            var userId = TestDataProvider.GetUserId(HttpContext.User.Identity.Name);

            //Если места не заняты, добавляем информацию о бронировании и привязывем ее к текущему пользователю
            foreach (var seat in bookedSeats)
            {
                var booking = new BookingViewModel();
                booking.Schedule = schedule;
                booking.Seat = seat;
                booking.BookingCode = "123412341234";
                TestDataProvider.AddBooking(userId, booking);
            }

            return new OkResult(); //Status code 200
        }
    }
}