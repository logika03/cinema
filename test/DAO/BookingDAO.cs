using Npgsql;
using System;
using System.Collections.Generic;
using cinema.Models;
using cinema.DAO;

namespace cinema.DAO
{
    public class BookingDAO
    {
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

        public static List<BookingViewModel> GetBookingByScheduleId(int idSchedule)
        {
            var result = new List<BookingViewModel>();
            var sqlExpression = string.Format("SELECT code, nickname, row, place FROM tickets JOIN users ON " +
                "id_user = users.id AND id_schedule ={0}", idSchedule);
            void addValues(NpgsqlDataReader reader)
            {
                result.Add(new BookingViewModel()
                {
                    BookingCode = reader.GetValue(0).ToString(),
                    UserNickName = reader.GetValue(1).ToString(),
                    Row = Convert.ToInt16(reader.GetValue(2)),
                    Seat = Convert.ToInt16(reader.GetValue(3))

                });
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

        public static int FindPrice(string sqlExpression)
        {
            var value = 0;
            void addValues(NpgsqlDataReader reader)
            {
                if (!(reader is null))
                    value = (int)reader.GetValue(0);
            }
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return value;
        }
    }
}
