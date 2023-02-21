using System.Net;

namespace DamianGonzalezCSharp.Models
{
    public class Login
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public Login (string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }

    public class LoginResponse : Response
    {
        public string Token { get; set; } = "";
        public LoginResponse()
        {
            Success = true;
            Message = "";
            StatusCode = HttpStatusCode.OK;
            Token = "";
        }

        public LoginResponse (Boolean success, string message, HttpStatusCode statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Token = "";
        }   
    }
}
