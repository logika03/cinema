using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test.Models;
using test.DAO;

namespace test.DAO
{
    public class DAOFactory
    {
        private static readonly string connectionString = string.Format("Server={0};Port={1};Username={2};Password={3};Database={4};",
               "localhost", "5432", "postgres", "postgres", "cinema");

        public static void ToHandleRequest(string sqlExpression, Action<NpgsqlDataReader> addValues)
        {
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                    addValues(reader);
            }
            connection.Close();
        }

        public static void AddData(string sqlExpression)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(sqlExpression.ToString(), connection);
            command.ExecuteNonQuery();
            connection.Close();
        }


        public static bool Contains(string sqlExpression)
        {
            var flag = false;
            void addValues(NpgsqlDataReader reader)
            {
                flag = Convert.ToInt32(reader.GetValue(0)) != 0;
            }
            ToHandleRequest(sqlExpression, addValues);
            return flag;
        }

        public static int FindPrice(string sqlExpression)
        {
            var value = 0;
            void addValues(NpgsqlDataReader reader)
            {
                if (!(reader is null))
                    value = (int)reader.GetValue(0);
            }
            ToHandleRequest(sqlExpression, addValues);
            return value;
        }

        public static List<BookingViewModel> GetBooknig(int id_schedule)
        {
            var result = new List<BookingViewModel>();
            var sqlExpression = string.Format("SELECT code, nickname, row, place FROM tickets JOIN users ON " +
                "id_user = users.id AND id_schedule ={0}", id_schedule);
            void addValues(NpgsqlDataReader reader)
            {
                //var scheduleId = (int)reader.GetValue(0);
                result.Add(new BookingViewModel()
                {
                    BookingCode = reader.GetValue(0).ToString(),
                    UserNickName = reader.GetValue(1).ToString(),
                    Schedule = FilmViewModelDAO.GetSchedule($"id_schedule = {id_schedule}").FirstOrDefault(),
                    Row = Convert.ToInt16(reader.GetValue(2)),
                    Seat = Convert.ToInt16(reader.GetValue(3))
                });
            }
            ToHandleRequest(sqlExpression, addValues);
            return result;
        }

        public static Tuple<int,string> GetUserIdByLoginPassword(string login, string password)
        {
            var id = -1;
            var name = "";
            var sqlExpression = string.Format("SELECT id, nickname FROM users WHERE nickname = '{0}' AND password = '{1}'", login, password);
            void addValues(NpgsqlDataReader reader)
            {
                if (reader.HasRows)
                {
                    id = (int)reader.GetValue(0);
                    name = reader.GetValue(1).ToString();
                }
            }
            ToHandleRequest(sqlExpression, addValues);
            return new Tuple<int,string>(id, name);
        }

        public static Dictionary<int, List<ScheduleViewModel>> GetIdFilmWithSchedule(DateTime date)
        {
            var schedules = FilmViewModelDAO.GetSchedule($"date::DATE = '{date:yyyy-MM-dd}'");
            var scheduleWithFilm = new Dictionary<int, List<ScheduleViewModel>>();
            foreach (var schedule in schedules)
                if (scheduleWithFilm.ContainsKey(schedule.Film.Id))
                    scheduleWithFilm[schedule.Film.Id].Add(schedule);
                else scheduleWithFilm.Add(schedule.Film.Id, new List<ScheduleViewModel> { schedule });
            return scheduleWithFilm;
        }
    }
}
