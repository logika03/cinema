using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test.Controllers;

namespace test.Pages
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
            {
                _authService.Logout();
                _authService.IsAuthenticated = false;
            }
            return Redirect(Url.Content("~/"));
        }
    }
}
