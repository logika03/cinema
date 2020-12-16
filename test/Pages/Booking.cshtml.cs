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
            BookingPageViewModel = new BookingPageViewModel
            {
                //Передаем сеанс с указаным id
                Schedule = FilmViewModelDAO.GetSchedule($"id_schedule = {id}").FirstOrDefault(),
                //Передаем все бронирования на этот сеанс
                BookingsInSchedule = DAOFactory.GetBooknig(id)
            };
            return Page();
        }

       /* public IActionResult OnPost(int id)
        {

        }*/

       
    }
}
