namespace cinema.Models
{
    public class RegistrationViewModel
    {
        public bool IsLoginUnavailable;
        public bool IsEmailUnavailable;
        public bool IsErrors;

        public string Login;
        public string Name;
        public string Surname;
        public string Email;

       /* public RegistrationViewModel(string login, string name, string surname, string email)
        {
            Login = login;
            Name = name;
            Surname = surname;
            Email = email;
        }*/
    }
}