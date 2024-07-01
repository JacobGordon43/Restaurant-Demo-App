using System.Xml.Linq;

namespace Restaurant_Demo_App.Models
{
    public class DayOpenModel
    {
        public string Id { get; set; }
        public string Day {  get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public bool IsClosed { get; set; }

        public DayOpenModel() { }

        public DayOpenModel(string id, string day, string openTime, string closeTime, bool isClosed)
        {
            Id = id;
            Day = day;
            OpenTime = openTime;
            CloseTime = closeTime;
            IsClosed = isClosed;
        }

        public DayOpenModel(string id, string day, bool isClosed)
        {
            Id = id;
            Day = day;
            IsClosed = isClosed;
        }

        public void ChangeHours(DayOpenModel hours)
        {
            Day = hours.Day;
            OpenTime = hours.OpenTime;
            CloseTime = hours.CloseTime;
            IsClosed = hours.IsClosed;
        }
    }

}
