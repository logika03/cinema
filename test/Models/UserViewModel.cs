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
        public string Password;

        public IEnumerable<BookingViewModel> Bookings;
        
        public string ImagePath;

        /*public UserViewModel(int id, string nickname, string name, string surname, string email, 
            string password, IEnumerable<BookingViewModel> bookings, string imagePath)
        {
            Id = id;
            NickName = nickname;
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            Bookings = bookings;
            ImagePath = imagePath;
        }*/
    }
}