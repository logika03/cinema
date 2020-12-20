using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema.Models;
using cinema.DAO;

namespace cinema.DAO
{
    public class DAOFactory
    {
        private static readonly string connectionString = $"Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=cinema;";

        public static void ToHandleRequest(string sqlExpression, Action<NpgsqlDataReader> addValues, params NpgsqlParameter[] parameters)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter);
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        addValues(reader);
                }

                connection.Close();
            }
        }

        public static void AddData(string sqlExpression)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression.ToString(), connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        
        public static object AddDataScalar(string sqlExpression)
        {
            object result = null;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression.ToString(), connection);
                result = command.ExecuteScalar();
                connection.Close();
            }

            return result;
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

        public static Dictionary<int, List<ScheduleViewModel>> GetIdFilmWithSchedule(DateTime date)
        {
            var schedules = ScheduleDAO.GetSchedule($"date::DATE = '{date:yyyy-MM-dd}'");
            var scheduleWithFilm = new Dictionary<int, List<ScheduleViewModel>>();
            foreach (var schedule in schedules)
                if (scheduleWithFilm.ContainsKey(schedule.FilmId))
                    scheduleWithFilm[schedule.FilmId].Add(schedule);
                else scheduleWithFilm.Add(schedule.FilmId, new List<ScheduleViewModel> { schedule });
            return scheduleWithFilm;
        }

        

       

        

       
    }
}
