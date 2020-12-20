using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace cinema
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public bool IsAuthenticated;
        public string Name;
        public int Id;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            Name = _httpContextAccessor.HttpContext.Session.GetString("Login");
            Id = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("Id"));
            IsAuthenticated = Name != null;
        }

        //Аутентификация пользователя(ставит в cookie информацию о пользователе
        //и позволяет потом получать ее через сервис)
        public void AuthenticateUser(string login, int id)
        {
            _httpContextAccessor.HttpContext.Session.SetString("Login", login);
            _httpContextAccessor.HttpContext.Session.SetString("Id", id.ToString());
            IsAuthenticated = true;
            Id = id;
            Name = login;
        }

        //Стирание аутентификационных cookie с клиента пользователя
        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove("Login");
            _httpContextAccessor.HttpContext.Session.Remove("Id");
            IsAuthenticated = false;
        }
    }
}