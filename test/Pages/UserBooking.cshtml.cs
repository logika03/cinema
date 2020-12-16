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
        public BookingPageViewModel BookingPageViewModel;
        public UserViewModel UserViewModel;
        public IActionResult OnGet(int id)
        {
            UserViewModel = FilmViewModelDAO.GetUserById(id);
            return Page();
        }
    }
}
