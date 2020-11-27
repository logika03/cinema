using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test.Models;

namespace test.Views.ProjectArt
{
    public class UserEditPageModel : PageModel
    {
        public BookingPageViewModel ViewModel;

        public IActionResult OnGet()
        {

            return Page();
        }

        public IActionResult OnPost()
        {

            return Page();
        }
    }
}