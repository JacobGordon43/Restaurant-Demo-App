namespace Restaurant_Demo_App.Models
{
    public class AdminModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public AdminModel(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public AdminModel() { }
    }
}
