using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DamianGonzalezCSharp.Models;
using DamianGonzalezCSharp;
using Microsoft.AspNetCore.Mvc;
using DamianGonzalesCSharp.Models;
using DamianGonzalesCSharp.Handlers;
using System.Net;
using System.Runtime.Intrinsics.Arm;

namespace DamianGonzalezCSharp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("GetUsers")]
        public UserResponse GetUsers(string? descrip, string? order)
        {
            List<User> users = new List<User>();
            DataSet dsUsers = new DataSet();
            UserHandler userHandler = new UserHandler();
            UserResponse response = new UserResponse();

            if (userHandler.HandleGetUsers(dsUsers, 0, descrip, order))
            {
                if (dsUsers.Tables.Contains("DAT") && dsUsers.Tables["DAT"].Rows.Count > 0)
                {
                    DataTable dtDAT = dsUsers.Tables["DAT"];
                    response.Success = true;
                    response.Message = "Successfull";
                    response.StatusCode = HttpStatusCode.OK;

                    foreach (DataRow dr in dtDAT.Rows)
                    {
                        User user = new User();

                        user.Id = Convert.ToInt32(dr["id"]);
                        user.Name = Convert.ToString(dr["nombre"]).Trim();
                        user.Surname = Convert.ToString(dr["apellido"]).Trim();
                        user.UserName = Convert.ToString(dr["nombreusuario"]).Trim();
                        user.Password = Convert.ToString(dr["contraseña"]).Trim();
                        user.Email = Convert.ToString(dr["mail"]).Trim();

                        users.Add(user);

                    }

                    response.Users = users;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Can't obtain requested data";
                    response.StatusCode = HttpStatusCode.Conflict;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Server not accessible";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }


            return response;
        }

        [HttpGet("GetUser")]
        public Response GetUser(Int32 userId)
        {
            User user = new User();
            List < User > userList = new List<User>();
            DataSet dsUser = new DataSet();
            UserHandler UserHandler = new UserHandler();
            UserResponse response = new UserResponse();

            if (UserHandler.HandleGetUsers(dsUser, userId, "", ""))
            {
                if (dsUser.Tables.Contains("DAT") && dsUser.Tables["DAT"].Rows.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Successfull";
                    response.StatusCode = HttpStatusCode.OK;
                    DataRow dr = dsUser.Tables["DAT"].Rows[0];
                    user.Id = Convert.ToInt32(dr["id"]);
                    user.Name = Convert.ToString(dr["nombre"]).Trim();
                    user.Surname = Convert.ToString(dr["apellido"]).Trim();
                    user.UserName = Convert.ToString(dr["nombreusuario"]).Trim();
                    user.Password = Convert.ToString(dr["contraseña"]).Trim();
                    user.Email = Convert.ToString(dr["mail"]).Trim();
                    userList.Add(user);
                    response.Users = userList;

                } else
                {
                    response.Success = false;
                    response.Message = "Can't obtain requested data";
                    response.StatusCode = HttpStatusCode.Conflict;
                }

            } else
            {
                response.Success = false;
                response.Message = "Server not accessible";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpPost("CreateUser")]
        public UserResponse CreateUser(User data)
        {
            UserResponse response = new UserResponse();
            UserHandler UserHandler = new UserHandler();

            if(UserHandler.HandleUserData(data, true))
            {
                response.Success = true;
                response.Message = "User created successfully";
                response.StatusCode = HttpStatusCode.OK;
            } else
            {
                response.Message = "An error has ocurred when inserting the User";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            
            return response;
        }

        [HttpPut("EditUser")]

        public UserResponse ModifyUser(User data)
        {
            UserResponse response = new UserResponse();
            UserHandler UserHandler = new UserHandler();

            if (data.Id != 0 && UserHandler.HandleUserData(data, false))
            {
                response.Success = true;
                response.Message = "User modified successfully";
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Message = "An error has ocurred when editing the User";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        [HttpDelete("DeleteUser")]

        public UserResponse DeleteUser(Int32 id)
        {
            UserResponse response = new UserResponse();
            UserHandler UserHandler = new UserHandler();

            if (id != 0 && UserHandler.HandleDeleteUser(id))
            {
                response.Success = true;
                response.Message = "User deleted successfully";
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.Message = "An error has ocurred when deleting the User";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}
