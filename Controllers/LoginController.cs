using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DamianGonzalezCSharp;
using DamianGonzalezCSharp.Handlers;
using DamianGonzalezCSharp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DamianGonzalezCSharp.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public LoginResponse LogIn(Login data)
        {
            LoginResponse response = new LoginResponse();
            LoginHandler handler = new LoginHandler();
            DataSet ds = new DataSet();

            if (handler.HandleLoginUser(ds, data))
            {
                if (ds.Tables.Contains("DAT") && ds.Tables["DAT"].Rows.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Successfull";
                    response.StatusCode = HttpStatusCode.OK;

                    response.Token = handler.HandleCreateToken();
                    
                }
                else
                {
                    response.Success = false;
                    response.Message = "Wrong credentials for login";
                    response.StatusCode = HttpStatusCode.Conflict;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Server not accessible";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            ds.Dispose();
            return response;
        }
    }
}
