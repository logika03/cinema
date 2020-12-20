using Npgsql;
using System;
using cinema.Models;
using cinema.DAO;

namespace cinema.DAO
{
    public class UserDAO
    {
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

        public static Tuple<int, string> GetUserIdByLoginPassword(string login, string password)
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
            DAOFactory.ToHandleRequest(sqlExpression, addValues);
            return new Tuple<int, string>(id, name);
        }

    }
}
