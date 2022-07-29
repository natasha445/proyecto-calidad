using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoCalidad.Models;
using ProyectoCalidad.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoCalidad.Database
{
    public class DatabaseConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DatabaseConnection()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "sql5.freemysqlhosting.net";
            database = "sql5508377";
            uid = "sql5508377";
            password = "dRSWdaRF7b";
            string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException)
            {
                return false;
            }
        }

        public List<EventModel> SelectEvents(string userName)
        {
            string query = "SELECT * FROM agendaevents WHERE userName = @userName";

            List<EventModel> list = new List<EventModel>();

            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", userName);

                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    EventModel eventObject = new EventModel();
                    eventObject.id = dataReader["id"].ToString();
                    eventObject.userName = dataReader["userName"].ToString();
                    eventObject.startMinutes = dataReader["startMinutes"].ToString();
                    eventObject.startHour = dataReader["startHour"].ToString();
                    eventObject.startDay = dataReader["startDay"].ToString();
                    eventObject.startMonth = dataReader["startMonth"].ToString();
                    eventObject.startYear = dataReader["startYear"].ToString();
                    eventObject.endMinutes = dataReader["endMinutes"].ToString();
                    eventObject.endHour = dataReader["endHour"].ToString();
                    eventObject.endDay = dataReader["endDay"].ToString();
                    eventObject.endMonth = dataReader["endMonth"].ToString();
                    eventObject.endYear = dataReader["endYear"].ToString();
                    eventObject.eventName = dataReader["eventName"].ToString();
                    eventObject.eventColor = dataReader["eventColor"].ToString();

                    list.Add(eventObject);
                }

                dataReader.Close();

                this.CloseConnection();
            }
            
            return list;
        }

        public int DuplicatedUser(string userName)
        {
            int result = 0;

            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand("duplicatedUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@inputUserName", userName);
                command.Parameters.Add("@ireturnvalue", MySqlDbType.Int32);
                command.Parameters["@ireturnvalue"].Direction = ParameterDirection.ReturnValue;

                command.ExecuteScalar();

                result = Convert.ToInt32(command.Parameters["@ireturnvalue"].Value);

                this.CloseConnection();
            }

            return result;
        }

        public List<UserModel> SelectUserDetails(string userName)
        {
            List<UserModel> list = new List<UserModel>();
            string query = "SELECT userPassword, isLocked, unlockDate FROM users WHERE userName = @userName";

            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@userName", userName);
                
                MySqlDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    UserModel userModel = new UserModel();

                    userModel.isLocked = dataReader["isLocked"].ToString();
                    userModel.unlockDate = dataReader["unlockDate"].ToString();
                    userModel.password = dataReader["userPassword"].ToString();

                    list.Add(userModel);
                }

                dataReader.Close();

                this.CloseConnection();
            }

            return list;
        }

        public void InsertUser(UserObject requestBody)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand("INSERT INTO users (userName, userPassword) VALUES (@userName, @userPassword)", connection);

                command.Parameters.AddWithValue("@userName", requestBody.UserName);
                command.Parameters.AddWithValue("@userPassword", requestBody.UserPassword);
                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void InsertEvent(EventObject requestBody)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand("INSERT INTO agendaevents (userName, startYear, startMonth, startDay, " +
                    "startHour, startMinutes, endYear, endMonth, endDay, endHour, endMinutes, eventName, eventColor)" +
                    " VALUES (@userName, @startYear, @startMonth, @startDay, @startHour, @startMinutes, @endYear, @endMonth, " +
                    "@endDay, @endHour, @endMinutes, @eventName, @eventColor)", connection);

                command.Parameters.AddWithValue("@userName", requestBody.UserName);
                command.Parameters.AddWithValue("@startYear", requestBody.StartYear);
                command.Parameters.AddWithValue("@startMonth", requestBody.StartMonth);
                command.Parameters.AddWithValue("@startDay", requestBody.StartDay);
                command.Parameters.AddWithValue("@startHour", requestBody.StartHour);
                command.Parameters.AddWithValue("@startMinutes", requestBody.StartMinutes);
                command.Parameters.AddWithValue("@endYear", requestBody.EndYear);
                command.Parameters.AddWithValue("@endMonth", requestBody.EndMonth);
                command.Parameters.AddWithValue("@endDay", requestBody.EndDay);
                command.Parameters.AddWithValue("@endHour", requestBody.EndHour);
                command.Parameters.AddWithValue("@endMinutes", requestBody.EndMinutes);
                command.Parameters.AddWithValue("@eventName", requestBody.EventName);
                command.Parameters.AddWithValue("@eventColor", requestBody.EventColor);
                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void InsertException(string eventName, string eventDate)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand("INSERT INTO logger (eventName, eventDate) VALUES (@eventName, @eventDate)", connection);

                command.Parameters.AddWithValue("@eventName", eventName);
                command.Parameters.AddWithValue("@eventDate", eventDate);
                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void InsertLockDetails(LockUserObject requestBody)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand("UPDATE users SET isLocked=@isLocked, unlockDate=@unlockDate WHERE userName=@userName", connection);

                command.Parameters.AddWithValue("@isLocked", requestBody.LockUser);
                command.Parameters.AddWithValue("@unlockDate", requestBody.UnlockDate);
                command.Parameters.AddWithValue("@userName", requestBody.UserName);
                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void UpdateEvent(EventObject requestBody)
        {
            string query = "UPDATE agendaevents SET startYear=@startYear, startMonth=@startMonth, startDay=@startDay, " +
                "startHour = @startHour, startMinutes=@startMinutes, endYear=@endYear, endMonth=@endMonth, endDay=@endDay, " +
                "endHour=@endHour, endMinutes=@endMinutes, eventName=@eventName, eventColor=@eventColor " +
                "WHERE id=@id AND userName=@userName";

            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", requestBody.Id);
                command.Parameters.AddWithValue("@userName", requestBody.UserName);
                command.Parameters.AddWithValue("@startYear", requestBody.StartYear);
                command.Parameters.AddWithValue("@startMonth", requestBody.StartMonth);
                command.Parameters.AddWithValue("@startDay", requestBody.StartDay);
                command.Parameters.AddWithValue("@startHour", requestBody.StartHour);
                command.Parameters.AddWithValue("@startMinutes", requestBody.StartMinutes);
                command.Parameters.AddWithValue("@endYear", requestBody.EndYear);
                command.Parameters.AddWithValue("@endMonth", requestBody.EndMonth);
                command.Parameters.AddWithValue("@endDay", requestBody.EndDay);
                command.Parameters.AddWithValue("@endHour", requestBody.EndHour);
                command.Parameters.AddWithValue("@endMinutes", requestBody.EndMinutes);
                command.Parameters.AddWithValue("@eventName", requestBody.EventName);
                command.Parameters.AddWithValue("@eventColor", requestBody.EventColor);

                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }

        public void RemoveEvent(EventObject requestBody)
        {
            string query = "DELETE FROM agendaevents WHERE id=@id";

            if (this.OpenConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@id", requestBody.Id);

                command.ExecuteNonQuery();

                this.CloseConnection();
            }
        }
    }
}
