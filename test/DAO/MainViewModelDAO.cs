using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Models;

namespace test.DAO
{
    public class MainViewModelDAO
    {
        public static List<FilmViewModel> GetTopFilms()
        {
            var films = new List<FilmViewModel>();
            var sqlExpression = "SELECT * FROM movies ORDER BY raiting DESC LIMIT 6";
            void addValues(NpgsqlDataReader reader)
            {
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return films;
        }
    }
}
