using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema.Models;

namespace cinema.DAO
{
    public class ScheduleDAO
    {
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
            /*foreach (var schedule in schedules)
                schedule.Hall = GetHallByScheduleId(schedule.Id);*/
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
            var result = new int[rowCount];
            var sqlExpression = string.Format($"SELECT count_places FROM rows JOIN halls_rows ON halls_rows.id_row = rows.id JOIN halls " +
                $"ON halls_rows.id_hall = halls.id AND halls.id ={id} ORDER BY rows.id");
            var i = 0;
            void addValues(NpgsqlDataReader reader)
            {
                result[i] = Convert.ToInt32(reader.GetValue(0));
                i++;
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return result;
        }
    }
}
