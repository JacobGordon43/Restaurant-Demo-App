using System.Configuration;

namespace Restaurant_Demo_App.Models
{
    public class InfoModel
    {
        public string PhoneNumber { get; set; }
        public List<DayOpenModel> BusinessHours { get; set; }
        public string Address { get; set; }
        [RegexStringValidator("\"|^http(s)?:\\/\\/[a-z0-9-]+(.[a-z0-9-]+)*(:[0-9]+)?(\\/.*)?$|i\"gm")]
        public string Instagram { get; set; }
        [RegexStringValidator("\"|^http(s)?:\\/\\/[a-z0-9-]+(.[a-z0-9-]+)*(:[0-9]+)?(\\/.*)?$|i\"gm")]
        public string Facebook { get; set; }
        [RegexStringValidator("\"|^http(s)?:\\/\\/[a-z0-9-]+(.[a-z0-9-]+)*(:[0-9]+)?(\\/.*)?$|i\"gm")]
        public string Twitter { get; set; }

        public InfoModel() 
        {
            PhoneNumber = string.Empty;
            BusinessHours = new List<DayOpenModel>();
            Instagram = string.Empty;
            Facebook = string.Empty;
            Twitter = string.Empty;
            Address = string.Empty;

        }
        public InfoModel(string phoneNumber, List<DayOpenModel> businessHours, string address, string instagram, string facebook, string twitter)
        {
            PhoneNumber = phoneNumber;
            BusinessHours = businessHours;
            Address = address;
            Instagram = instagram;
            Facebook = facebook;
            Twitter = twitter;
        }
    }
}
