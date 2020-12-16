using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.DAO;

namespace test.Controllers
{
    public class ProjectMovieController
    {
        /*public static AuthService AuthService { get; private set; }

        //Получаем сервис аунтификации через Dependency Injection
        public ProjectMovieController(AuthService authService)
        {
            AuthService = authService;
        }


        public static void A()
        {
            var a = AuthService;
            var b = 0;
        }
        public bool AddUsers(string login, string password)
        {
            var tupleIdName = DAOFactory.GetUserIdByLoginPassword(login, password);
            var id = tupleIdName.Item1;
            if (id > 0)
            {
                AuthService.Id = id;
                AuthService.IsAuthenticated = true;
                AuthService.Name = tupleIdName.Item2;
                return true;
            }
            return false;
        }*/


    }
}
