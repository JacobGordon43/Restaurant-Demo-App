using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Restaurant_Demo_App.Models
{
    public class AdminModel
    {
        public string Id { get; set; }
        public bool Authenticated { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [RegexStringValidator("\"^(?=.*\\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\\w\\d\\s:])([^\\s]){8,16}$\"gm")]
        public string Password { get; set; }

        public AdminModel(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
            Authenticated = false;
        }

        public AdminModel() 
        {
            Authenticated = false;
        }
    }
}
