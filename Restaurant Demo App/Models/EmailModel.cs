using System.ComponentModel.DataAnnotations;

namespace Restaurant_Demo_App.Models
{
    public class EmailModel
    {
        [EmailAddress(ErrorMessage = "You must enter a valid email address")]
        public string EmailAddress { get; set; }
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Must be 4 - 50 characters long")]
        public string Subject { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }

        public EmailModel(string emailAddress, string subject, string name, string message)
        {
            EmailAddress = emailAddress;
            Subject = subject;
            Name = name;
            Message = message;
        }

        public EmailModel()
        {

        }
    }
}
