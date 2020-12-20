using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace cinema.Pages
{
    public class ExitModel : PageModel
    {
        private readonly AuthService _authService;
        public ExitModel(AuthService authService)
        {
            _authService = authService;
        }
        public IActionResult OnGet()
        {
            if (_authService.IsAuthenticated)
                _authService.Logout();
            
            return Redirect(Url.Content("~/"));
        }
    }
}
