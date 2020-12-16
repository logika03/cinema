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
            var user = FilmViewModelDAO.GetUserById(id);
            if (_authService.IsAuthenticated
                && _authService.Name == user.NickName)
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
           var user = FilmViewModelDAO.GetUserById(id);
            if (_authService.IsAuthenticated
               && _authService.Name == user.NickName)
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

                //��� ����� �������� ���������������� cookie
                _authService.Logout();
                _authService.AuthenticateUser(nickname);
            }

            //���� ��� ������ ������ ��� 
            //������������ �� ����������� ��� id ����������� �� ���,
            //�� ������ �� �������� � ������������
            return Redirect(Url.Content($"~/user/{id}"));
        }
    }
}
