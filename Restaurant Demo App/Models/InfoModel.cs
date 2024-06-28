namespace Restaurant_Demo_App.Models
{
    public class InfoModel
    {
        public string PhoneNumber { get; set; }
        public List<DayOpenModel> BusinessHours { get; set; }
        public string Instagram { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public InfoModel() 
        {
            PhoneNumber = string.Empty;
            BusinessHours = new List<DayOpenModel>();
            Instagram = string.Empty;
            Facebook = string.Empty;
            Twitter = string.Empty;
        }
        public InfoModel(string phoneNumber, List<DayOpenModel> businessHours, string instagram, string facebook, string twitter)
        {
            PhoneNumber = phoneNumber;
            BusinessHours = businessHours;
            Instagram = instagram;
            Facebook = facebook;
            Twitter = twitter;
        }
    }
}
