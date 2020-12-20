using Npgsql;
using System;
using System.Collections.Generic;
using cinema.Models;
using cinema.DAO;

namespace cinema.DAO
{
    public class FilmViewModelDAO
    {
        public static Dictionary<int, FilmViewModel> Films = 
              new Dictionary<int, FilmViewModel>();

        public static List<FilmViewModel> GetFilms(string additionalCondition, bool needTwoGenres, params NpgsqlParameter[] parameters)
        {
            var films = new List<FilmViewModel>();
            var sqlExpression = string.Format("SELECT id, title, id_country, release_year, duration, description, " +
                "image_path, raiting FROM movies {0}", additionalCondition);
            var idCountry = 0;
            void addValues(NpgsqlDataReader reader)
            {
                idCountry = (int)reader.GetValue(2);
                var id = (int)reader.GetValue(0);
                var film = new FilmViewModel()
                {
                    Id = id,
                    Name = reader.GetValue(1).ToString(),
                    Description = reader.GetValue(5).ToString(),
                    Year = (int)reader.GetValue(3),
                    ImagePath = reader.GetValue(6).ToString(),
                    DurationInMinutes = Convert.ToInt16(reader.GetValue(4)),
                    CountryId = idCountry,
                    Rating = Math.Round(Convert.ToDouble(reader.GetValue(7)), 2)
                };
                films.Add(film);
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues, parameters);

            foreach (var film in films)
                film.Genres = GetGenresByFilmId(film.Id, needTwoGenres);
            return films;
        }

        public static List<string> GetProducersByFilmId(int id)
            => GetPersonsByFilmId(id, "producers", "movies_producers", "id_producer");

        public static List<string> GetActorsByFilmId(int id)
            => GetPersonsByFilmId(id, "actors", "actors_movies", "id_actor");

        private static List<string> GetPersonsByFilmId(int id, string tableName, string stagingTableName, string idPerson)
        {
            var producers = new List<string>();
            var sqlExpression = string.Format("SELECT firstname, lastname FROM {0} INNER JOIN {1} ON " +
                "{1}.{2} = {0}.id INNER JOIN movies ON movies.id = {3} and {1}.id_movie = movies.id", 
                tableName, stagingTableName, idPerson, id);
            void addValues(NpgsqlDataReader reader)
            {
                producers.Add(string.Format("{0} {1}", reader.GetValue(0).ToString(), reader.GetValue(1).ToString()));
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return producers;
        }

        public static string GetCounrtyById(int id)
        {
            var sqlExpression = string.Format("SELECT name FROM countries WHERE id = {0}", id);
            var name = "";
            void addValues(NpgsqlDataReader reader)
            {
                name = reader.GetValue(0).ToString();
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return name;
        }         

        public static List<ReviewViewModel> GetReviewByFilmId(int id)
        {
            var result = new List<ReviewViewModel>();
            var sqlExpression = string.Format("SELECT text, date, raiting, author " +
                "FROM reviews WHERE id_movie = {0}", id);
            void addValues(NpgsqlDataReader reader)
            {
                var userId = (int)reader.GetValue(3);
                var review = new ReviewViewModel()
                {
                    UserId = userId,
                    ReviewText = reader.GetValue(0).ToString(),
                    TimeOfReview = (DateTime)reader.GetValue(1),
                    Rating = Convert.ToInt16(reader.GetValue(2))
                };
                result.Add(review);
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);

            foreach (var review in result)
                review.User = UserDAO.GetUserById(review.UserId);

            return result;
        }        

        public static List<string> GetGenresByFilmId(int id, bool needTwoGenres)
        {
            var result = new List<string>();
            var sqlExpression = string.Format("SELECT name FROM genres WHERE id IN (SELECT genre_id FROM movies_genres " +
                "WHERE movie_id = {0})", id);
            var count = 0;
            void addValues(NpgsqlDataReader reader)
            {
                if (count < 2 && needTwoGenres || !needTwoGenres)
                    result.Add(reader.GetValue(0).ToString());
                count++;
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return result;
        }
    }
}
