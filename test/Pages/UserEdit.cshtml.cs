using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using cinema.Models;
using cinema.DAO;

namespace cinema.Pages
{
    public class UserEditModel : PageModel
    {
        public UserViewModel UserViewModel;
        private readonly AuthService _authService;
        public UserEditModel(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult OnGet(int id)
        {
            var user = UserDAO.GetUserById(id);
            if (_authService.IsAuthenticated
                && _authService.Id == id)
            {
                ViewData["login_unavailable"] = false;
                ViewData["email_unavailable"] = false;
                ViewData["validation_errors"] = false;

                UserViewModel = user;
                return Page();
            }

            //���� ������������ �� ����������� ��� id ����������� �� ���,
            //�� ������ �� �������� � ������������
            return Redirect(Url.Content($"~/user/{id}"));
        }

        public IActionResult OnPost(
            int id,
            string nickname,
            string name,
            string surname,
            string email)
        {
            var user = UserDAO.GetUserById(id);
            if (_authService.IsAuthenticated && _authService.Id == id)
            {
                var loginUnavailable = false; //�����(�������) �����
                var emailUnavailable = false; //Email �����
                var isErrors = false; //������ � ������(�������� �� ��� ������ email � �.�)

                if (email != user.Email
                    && DAOFactory.Contains(string.Format("SELECT COUNT(*) FROM users WHERE email = '{0}'", email)))
                    emailUnavailable = true;
                if (nickname != user.NickName
                    && DAOFactory.Contains(string.Format("SELECT COUNT(*) FROM users WHERE nickname = '{0}'", nickname)))
                    loginUnavailable = true;
                //Validate input, if errors - set isErrors = true

                ViewData["login_unavailable"] = loginUnavailable;
                ViewData["email_unavailable"] = emailUnavailable;
                ViewData["validation_errors"] = isErrors;

                //���� ���� ������, ���������� �� �������� ��������������
                if (loginUnavailable || emailUnavailable || isErrors)
                {
                    UserViewModel = user;
                    return Page();
                }

                DAOFactory.AddData(string.Format("UPDATE users SET nickname = '{0}', name = '{1}', surname = '{2}', email = '{3}' WHERE id = {4}",
                    nickname, name, surname, email, id));
                user.Name = name;
                user.Surname = surname;
                user.Email = email;
                user.NickName = nickname;
                _authService.Name = nickname;

                //�������� ������������������ cookie
                _authService.Logout();
                _authService.AuthenticateUser(nickname, id);
            }

            //���� ��� ������ ������ ��� 
            //������������ �� ����������� ��� id ����������� �� ���,
            //�� ������ �� �������� � ������������
            return Redirect(Url.Content($"~/user/{id}"));
        }
    }
}
