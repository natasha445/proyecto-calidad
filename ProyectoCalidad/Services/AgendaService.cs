using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoCalidad.Database;
using ProyectoCalidad.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCalidad.Services
{
    public class AgendaService
    {
        private DatabaseConnection databaseConnection = new DatabaseConnection();

        public IActionResult getUserEvents(UserEventObject requestBody)
        {
            int statusCode;
            string result;

            try
            {
                result = JsonConvert.SerializeObject(databaseConnection.SelectEvents(requestBody.UserName));
                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                result = "An error ocurred with the server, try again later";
               
                databaseConnection.InsertException($"{ex.Message} - agenda, getUserEvents", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                Content = result,
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public IActionResult RegisterEvent(EventObject requestBody)
        {
            string result;
            int statusCode;

            try
            {
                databaseConnection.InsertEvent(requestBody);
                result = "Event was registered successfully";

                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result = "An error occured while registering the event";
                statusCode = (int)HttpStatusCode.BadRequest;

                databaseConnection.InsertException($"{ex.Message} - agenda, registerEvent", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                Content = result,
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public IActionResult UpdateEvent(EventObject requestBody)
        {
            string result;
            int statusCode;

            try
            {
                databaseConnection.UpdateEvent(requestBody);
                result = "Event was updated successfully";

                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result = "An error occured while updating the event";
                statusCode = (int)HttpStatusCode.BadRequest;

                databaseConnection.InsertException($"{ex.Message} - agenda, updateEvent", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                Content = result,
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public IActionResult RemoveEvent(EventObject requestBody)
        {
            string result;
            int statusCode;

            try
            {
                databaseConnection.RemoveEvent(requestBody);
                result = "Event was removed successfully";

                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result = "An error occured while removing the event";
                statusCode = (int)HttpStatusCode.BadRequest;

                databaseConnection.InsertException($"{ex.Message} - agenda, removeEvent", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                Content = result,
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }
    }
}
