using System.Data;
using DamianGonzalezCSharp.Models;
using Microsoft.AspNetCore.Mvc;
using DamianGonzalezCSharp.Handlers;
using System.Net;
using DamianGonzalezCSharp.Validations;

namespace DamianGonzalezCSharp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly UserValidations validations = new();
        private LoginHandler loginHandler = new LoginHandler();

        [HttpGet("GetUsers")]
        public UserResponse GetUsers(string? descrip, string? order)
        {
            List<User> users = new List<User>();
            DataSet dsUsers = new DataSet();
            UserHandler userHandler = new UserHandler();
            UserResponse response = new UserResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);

            if (userHandler.HandleGetUsers(dsUsers, descrip, order))
            {
                if (dsUsers.Tables.Contains("DAT") && dsUsers.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsUsers.Tables["DAT"];

                    foreach (DataRow dr in dtDAT.Rows)
                    {
                        User user = new User();

                        user.Id = Convert.ToInt32(dr["id"]);
                        user.Name = Convert.ToString(dr["nombre"]).Trim();
                        user.Surname = Convert.ToString(dr["apellido"]).Trim();
                        user.UserName = Convert.ToString(dr["nombreusuario"]).Trim();
                        user.Email = Convert.ToString(dr["mail"]).Trim();

                        users.Add(user);

                    }
                    response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
                    response.Users = users;
                    
                }
                else
                {
                    response.GenerateResponse(false, "Can't obtain requested data", HttpStatusCode.Conflict);
                }
            }

            dsUsers.Dispose();
            return response;
        }

        [HttpGet("GetUser")]
        public UserResponse GetUser(Int32 userId)
        {
            User user = new User();
            List < User > userList = new List<User>();
            DataSet dsUser = new DataSet();
            UserHandler UserHandler = new UserHandler();
            UserResponse response = new UserResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);

            if (UserHandler.HandleGetUsers(dsUser, userId))
            {
                if (dsUser.Tables.Contains("DAT") && dsUser.Tables["DAT"].Rows.Count > 0)
                {
                    DataRow dr = dsUser.Tables["DAT"].Rows[0];
                    user.Id = Convert.ToInt32(dr["id"]);
                    user.Name = Convert.ToString(dr["nombre"]).Trim();
                    user.Surname = Convert.ToString(dr["apellido"]).Trim();
                    user.UserName = Convert.ToString(dr["nombreusuario"]).Trim();
                    user.Email = Convert.ToString(dr["mail"]).Trim();
                    userList.Add(user);

                    response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
                    response.Users = userList;
                    

                } else
                {
                    response.GenerateResponse(false, "Can't obtain requested data", HttpStatusCode.Conflict);
                }

            }

            dsUser.Dispose();
            return response;
        }

        [HttpPost("CreateUser")]
        public UserResponse CreateUser(User data)
        {
            UserResponse response = validations.ValidateCreation(data);
            UserHandler UserHandler = new UserHandler();

            if(response.Success == true && UserHandler.HandleCreateUser(data))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }
            
            return response;
        }

        [HttpPut("EditUser")]
        public UserResponse ModifyUser(User data)
        {
            UserResponse response = validations.ValidateUpdate(data);
            UserHandler UserHandler = new UserHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }

            if (response.Success == true && UserHandler.HandleUpdateUser(data))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpPut("EditPassword")]
        public UserResponse ModifyPassword(UserPassword data)
        {
            UserResponse response = validations.ValidatePassword(data);
            UserHandler UserHandler = new UserHandler();

            if (response.Success == true && UserHandler.HandleUpdatePassword(data))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpDelete("DeleteUser")]

        public UserResponse DeleteUser(Int32 id)
        {
            UserResponse response = validations.ValidateDelete(id);
            UserHandler UserHandler = new UserHandler();

            if (!loginHandler.HandleGetToken(Request.Headers))
            {
                response.GenerateResponse(false, "Unauthorized", HttpStatusCode.Unauthorized);
            }

            if (response.Success == true && UserHandler.HandleDeleteUser(id))
            {
                response.GenerateResponse(true, "Successfull", HttpStatusCode.OK);
            }
            else if (response.Success == true)
            {
                response.GenerateResponse(false, "There was a problem communicating with the server", HttpStatusCode.InternalServerError);
            }

            return response;
        }
    }
}
