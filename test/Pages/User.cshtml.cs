using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using cinema.Models;
using cinema.DAO;

namespace cinema.Pages
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
            //Если id пользователя не указан, то нужно отобразить страницу текущего пользователя, если он авторизован
            UserViewModel = new UserViewModel();
            if (id == null)
            {
                if (_authService.IsAuthenticated)
                {
                    var user = UserDAO.GetUserById(_authService.Id);
                    if (user is null)
                        return BadRequest();
                    UserViewModel = user;
                    return Page();
                }
                return NotFound();
            }

            UserViewModel = UserDAO.GetUserById(id.Value);
            if (UserViewModel == null)
                return NotFound();

            return Page();
        }
    }
}
