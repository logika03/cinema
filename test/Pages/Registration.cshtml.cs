using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test.Models;
using test.DAO;
using test.Controllers;

namespace test.Pages
{
    public class RegistrationModel : PageModel
    {
        public BookingPageViewModel ViewModel;
        public RegistrationViewModel RegistrationViewModel = new RegistrationViewModel();
        private readonly AuthService _authService;
        public RegistrationModel(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            if (_authService.IsAuthenticated)
                return Redirect(Url.Content("~/user"));
            return Page();//model with default values
        }
        public IActionResult OnPostRegistration(string login, string name, string surname,
            string email, string password,
            string confirmationPassword, bool terms)
        {
            RegistrationViewModel = new RegistrationViewModel()
            {
                Email = email,
                Login = login,
                Name = name,
                Surname = surname
            };
            //Validate email, login, name, surname, password, etc.
            //if errors set RegistrationViewModel.IsErrors = true
            if (DAOFactory.Contains(string.Format("SELECT COUNT(*) FROM users WHERE email = {1}" , email)))
                RegistrationViewModel.IsEmailUnavailable = true;
            if (DAOFactory.Contains(string.Format("SELECT COUNT(*) FROM users WHERE nickname = {1}", login)))
                RegistrationViewModel.IsLoginUnavailable = true;
            if (RegistrationViewModel.IsErrors || RegistrationViewModel.IsEmailUnavailable || RegistrationViewModel.IsLoginUnavailable)
                return Page();
            //If registration went successfully -
            //add user to db, authorize him and redirect to main page
            RegisterUser(login, name, surname, email, password);
            _authService.AuthenticateUser(login);
            return Redirect(Url.Content("~/"));
        }

        private void RegisterUser(string login, string name, string surname,
            string email, string password)
        {
            var imagePath = "~/images/users/user.png";
            var sqlExpression = string.Format("INSERT INTO users(name, surname, nickname, email, password, image_path) VALUES " +
                "('{0}','{1}','{2}', '{3}', '{4}', '{5}')", name, surname, login, email, password, imagePath);
            DAOFactory.AddData(sqlExpression);
        }
    }
}
