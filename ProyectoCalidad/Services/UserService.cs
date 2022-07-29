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
    public class UserService
    {
        private DatabaseConnection databaseConnection = new DatabaseConnection();

        public IActionResult ValidateCredentials(UserObject requestBody)
        {
            int statusCode;
            string result;

            try
            {
                result = JsonConvert.SerializeObject(databaseConnection.SelectUserDetails(requestBody.UserName));
                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                result = "Error";
                
                databaseConnection.InsertException($"{ex.Message} - users, validateCredentials", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                Content = result,
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public IActionResult SignUp(UserObject requestBody)
        {
            string result;
            int statusCode;

            try
            {
                int userIsDuplicated = databaseConnection.DuplicatedUser(requestBody.UserName);
                
                if (userIsDuplicated == 1)
                {
                    result = "That username is already in use";
                } 
                else
                {
                    databaseConnection.InsertUser(requestBody);
                    result = "User was registered successfully";
                }

                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result = "An error occured while registering user";
                statusCode = (int)HttpStatusCode.BadRequest;
                databaseConnection.InsertException($"{ex.Message} - users, signUp", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                Content = result,
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public IActionResult FailedLogin(UserObject requestBody)
        {
            int statusCode;

            try
            {
                databaseConnection.InsertException($"Attemp to login failed, user: {requestBody.UserName}", DateTime.Now.ToString());
                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                databaseConnection.InsertException($"{ex.Message} - users, FailedLogin", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public IActionResult LockUser(LockUserObject requestBody)
        {
            int statusCode;

            try
            {
                databaseConnection.InsertLockDetails(requestBody);
                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                databaseConnection.InsertException($"{ex.Message} - users, LockUser", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }

        public IActionResult UnlockUser(LockUserObject requestBody)
        {
            int statusCode;

            try
            {
                databaseConnection.InsertLockDetails(requestBody);
                statusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                databaseConnection.InsertException($"{ex.Message} - users, UnlockUser", DateTime.Now.ToString());
            }

            return new ContentResult
            {
                StatusCode = statusCode,
                ContentType = "application/json"
            };
        }
    }
}