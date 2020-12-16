using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test.Models;
using test.DAO;
using test.Controllers;
using System.Diagnostics;

namespace test.Pages
{
    public class UserModel : PageModel
    {
        public UserViewModel UserViewModel;
        private readonly AuthService _authService;
        public UserModel(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult OnGet(int? id)
        {
            //Если id пользователя не указан, то есть страница была вызвана по адресу ~/user(без /{id})
            //То нужно отобразить страницу текущего пользователя, если он авторизован
            UserViewModel = new UserViewModel();
            if (_authService.IsAuthenticated)
            {
                var user = FilmViewModelDAO.GetUserById(_authService.Id);
                if (user is null)
                    return BadRequest();
                //Если id указан, показываем страницу пользователя с данным id
                UserViewModel = user;
                return Page();
            }
            //Если пользователь еще не вошел в систему, кидаем на главную страницу
            return NotFound();
        }
    }
}
