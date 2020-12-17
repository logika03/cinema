using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test.Controllers;
using test.DAO;

namespace test.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;
        public LoginModel(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult OnGet(string login, string password)
        {
            var hashedPassword = password.GetSHA256Hash();
            var done = AddUsers(login, hashedPassword);
            if (done)
                return new OkResult();
            return BadRequest();
        }

        public IActionResult OnPost(string login, string password) // string login, string password)
        {
            var hashedPassword = password.GetSHA256Hash();
            var done = AddUsers(login, hashedPassword);
            if (done)
                return new OkResult();
            return BadRequest();
        }

        private bool AddUsers(string login, string password)
        {
            var tupleIdName = DAOFactory.GetUserIdByLoginPassword(login, password);
            var id = tupleIdName.Item1;
            if (id > 0)
            {
                _authService.AuthenticateUser(login, id);
                return true;
            }
            return false;
        }
    }
}
