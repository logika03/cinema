using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace test
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public bool IsAuthenticated;
        public string Name;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            Name = _httpContextAccessor.HttpContext.Session.GetString("Login");
            IsAuthenticated = Name != null;
        }

        //Аунтификация пользователя(ставит в cookie информацию о пользователе
        //и позволяет потом получать ее через сервис)
        public void AuthenticateUser(string login)
        {
            _httpContextAccessor.HttpContext.Session.SetString("Login", login);
        }

        //Стирание аунтификационных cookie с клиента пользователя
        public void Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove("Login");
        }
    }
}