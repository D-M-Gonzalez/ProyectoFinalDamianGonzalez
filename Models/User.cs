using DamianGonzalesCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamianGonzalezCSharp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string UserName { get; set; } = ""; 
        public string Password { get; set; } = "";
        public string Email { get; set; } = "";
    }

    public class UserResponse : Response
    {
        public List<User>? Users { get; set; }
    }
}
