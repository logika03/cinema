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
        public static List<FilmViewModel> GetFilms(string condition)
        {
            var films = new List<FilmViewModel>();
            var sqlExpression = $"SELECT id, title, image_path, release_year FROM movies WHERE is_rent = 1 {condition}";
            void addValues(NpgsqlDataReader reader)
          {
                var film = new FilmViewModel()
                {
                    Id = (int)reader.GetValue(0),
                    Name = reader.GetValue(1).ToString(),
                    ImagePath = reader.GetValue(2).ToString(),
                    Year = (int)reader.GetValue(3)
                };
                films.Add(film);
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            foreach (var film in films)
            {
                film.Genres = FilmViewModelDAO.GetGenresByFilmId(film.Id, true);
            }
            return films;
        }

    }
}
