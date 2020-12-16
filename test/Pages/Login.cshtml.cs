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
        public IActionResult OnGet()
        {
            var login = "logika";
            var password = "12aZ";
            var done = AddUsers(login, password);
            if (done)
                return Redirect("/");
            return Redirect("/");
        }

        public IActionResult OnPost(params object[] arg) // string login, string password)
        {
            var login = "logika";
            var password = "12aZ";
            var done = AddUsers(login, password);
            if (done)
                return Redirect(string.Format("~/user/{0}", _authService.Id));
            return Redirect("/");
        }

        private bool AddUsers(string login, string password)
        {
            var tupleIdName = DAOFactory.GetUserIdByLoginPassword(login, password);
            var id = tupleIdName.Item1;
            if (id > 0)
            {
                _authService.Id = id;
                _authService.IsAuthenticated = true;
                _authService.Name = tupleIdName.Item2;
               /* ProjectMovieController.AuthService.Id = id;
                ProjectMovieController.AuthService.IsAuthenticated = true;
                ProjectMovieController.AuthService.Name = tupleIdName.Item2;*/
                return true;
            }
            return false;
        }
    }
}
