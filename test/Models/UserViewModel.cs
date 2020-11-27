using System.Collections;
using System.Collections.Generic;

namespace test.Models
{
    public class UserViewModel
    {
        public int Id;
        
        public string NickName; //Login
        public string Name;
        public string Surname;
        public string Email;

        public IEnumerable<BookingViewModel> Bookings;
        
        public string ImagePath;
    }
}