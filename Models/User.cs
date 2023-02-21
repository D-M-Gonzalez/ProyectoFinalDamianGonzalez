
using System.Net;

namespace DamianGonzalezCSharp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }

    public class UserPassword
    {
        public string UserName { get; set; } = "";
        public string NewPassword { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class UserResponse : Response
    {
        public List<User>? Users { get; set; }

        public UserResponse()
        {
            Success = true;
            Message = "";
            StatusCode = HttpStatusCode.OK;
            Users = null;
        }
        public UserResponse(Boolean success, string message, HttpStatusCode statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Users = null;
        }
    }
}
