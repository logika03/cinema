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

        //�������� ������������ ����
        //Id - ��� id ������(Schedule)
        private readonly AuthService _authService;
        public BookingModel(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult OnGet(int id)
        {
            Id = id;
            //�� ����������� - �� ������� �������
            if (!_authService.IsAuthenticated)
                return Redirect(Url.Content("~/"));

            var schedule = FilmViewModelDAO.GetSchedule($"id_schedule = {id}").FirstOrDefault();
            schedule.Hall = FilmViewModelDAO.GetHallByScheduleId(id);

            var bookings = DAOFactory.GetBooknig(id);
            foreach (var booking in bookings)
                booking.Schedule = schedule;

            BookingPageViewModel = new BookingPageViewModel
            {
                //�������� ����� � �������� id
                Schedule = schedule,
                //�������� ��� ������������ �� ���� �����
                BookingsInSchedule = bookings
            };
            return Page();
        }

       /* public IActionResult OnPost(int id)
        {

        }*/

       
    }
}
