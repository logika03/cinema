using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace cinema.Pages
{
    public class TermsOfUseModel : PageModel
    {
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
