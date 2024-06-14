namespace Restaurant_Demo_App.Models
{
    public class AdminModel
    {
        private string Name { get; set; }
        private string Email { get; set; }
        private string Password { get; set; }

        public AdminModel(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
