using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using test.Models;

namespace test.DAO
{
    public class FilmViewModelDAO
    {
        public static Dictionary<int, FilmViewModel> Films = 
              new Dictionary<int, FilmViewModel>();

        public static List<FilmViewModel> GetFilms(string additionalCondition, bool needTwoGenres)
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
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            
            /*foreach(var film in films)
            {
                var id = film.Id;
                film.Country = GetCounrtyById(film.CountryId);
                film.Actors = GetActorsByFilmId(id);
                film.Producers = GetProducersByFilmId(id);
                film.Reviews = GetReviewByFilmId(id);
            }*/

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

        public static List<ScheduleViewModel> GetSchedule(string str)
        {
            var schedules = new List<ScheduleViewModel>();
            var sqlExpression = string.Format("SELECT id_schedule, fix_price, date, id_movie" +
                " FROM schedule WHERE {0}", str);
            void addValues(NpgsqlDataReader reader)
            {
                var scheduleId = (int)reader.GetValue(0);
                var pricePerSeat = Convert.ToDecimal(reader.GetValue(1));
                var time = (DateTime)reader.GetValue(2);
                // var hall = GetHallByScheduleId(scheduleId);
                var schedule = new ScheduleViewModel()
                {
                    Id = scheduleId,
                    PricePerSeat = pricePerSeat,
                    Time = time,
                    FilmId = (int)reader.GetValue(3)
                };
                schedules.Add(schedule);
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            foreach (var schedule in schedules)
                schedule.Hall = GetHallByScheduleId(schedule.Id);
            return schedules;
        }

        public static HallViewModel GetHallByScheduleId(int id)
        {
            var hall = new HallViewModel();
            var sqlExpression = string.Format("SELECT id, count_row FROM halls WHERE id = (SELECT id_hall FROM schedule WHERE id_schedule = {0})", id);
            void addValues(NpgsqlDataReader reader)
            {
                var hallId = (int)reader.GetValue(0);
                var rowCount = Convert.ToInt16(reader.GetValue(1));
                hall.HallNumber = hallId;
                hall.RowCount = rowCount;
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            hall.SeatsRowCount = GetSeatsRowCountByHallId(hall.HallNumber, hall.RowCount);
            return hall;
        }

        public static int[] GetSeatsRowCountByHallId(int id, int rowCount)
        {
            /* Debug.WriteLine("seats");
             var result = new int[rowCount + 1];
             var sqlExpression = string.Format("SELECT row_number, count_places FROM rows JOIN halls_rows ON rows.id = id_row " +
                 "JOIN halls ON halls.id = {0} and halls.id = id_hall", id);
             void addValues(NpgsqlDataReader reader)
             {
                 result[Convert.ToInt16(reader.GetValue(0))] = Convert.ToInt32(reader.GetValue(1));
             }
             DAOFactory.ToHandleRequest(sqlExpression, addValues);*/
            var result = new int[5] { 10, 10, 10, 10, 10 };
            return result;
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
                    //GetUserById(userId)
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
                review.User = GetUserById(review.UserId);

            return result;
        }

        public static UserViewModel GetUserById(int id)
        {
            UserViewModel result = null;
            var sqlExpression = string.Format("SELECT id, nickname, name, surname, email, password, image_path" +
                " FROM users WHERE id = {0}", id);
            void addValues(NpgsqlDataReader reader)
            {
                if (!(reader is null))
                {
                    result = new UserViewModel()
                    {
                        Id = (int)reader.GetValue(0),
                        NickName = reader.GetValue(1).ToString(),
                        Name = reader.GetValue(2).ToString(),
                        Surname = reader.GetValue(3).ToString(),
                        Email = reader.GetValue(4).ToString(),
                        Password = reader.GetValue(5).ToString(),
                        ImagePath = reader.GetValue(6).ToString()
                    };
                }
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return result;
        }

        public static List<BookingViewModel> GetBookingsByUserId(int id)
        {
            var result = new List<BookingViewModel>();
            var sqlExpression = string.Format("SELECT id, code, row, place, id_schedule" +
                " FROM tickets WHERE id_user = {0}", id);

            var userNickName = GetNickNameUserById(id);
            
            void addValues(NpgsqlDataReader reader)
            {
                var idSchedule = (int)reader.GetValue(4);
                var booking = new BookingViewModel()
                {
                    Id = (int)reader.GetValue(0),
                    BookingCode = reader.GetValue(1).ToString(),
                    ScheduleId = idSchedule,
                    Row = Convert.ToInt32(reader.GetValue(2)),
                    Seat = Convert.ToInt32(reader.GetValue(3)),
                    UserNickName = userNickName
               }; 
                
               result.Add(booking);
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return result;
        }

        public static string GetNickNameUserById(int id)
        {
            var result = "";
            var sqlExpression = string.Format("SELECT nickname FROM users WHERE id = {0}", id);
            void addValues(NpgsqlDataReader reader)
            {
                result = reader.GetValue(0).ToString();
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
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
