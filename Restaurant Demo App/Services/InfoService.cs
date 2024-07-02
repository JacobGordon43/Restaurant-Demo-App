using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restaurant_Demo_App.Models;
using System.Diagnostics;

namespace Restaurant_Demo_App.Services
{
    public class InfoService
    {
        public static InfoModel info;

        public InfoService(){}
        public void SetInfo()
        {
            InfoModel GeneralInfo = new InfoModel();
            try
            {
                using (StreamReader r = new StreamReader("wwwroot/json/info.json"))
                {
                    string file = r.ReadToEnd();
                    Debug.WriteLine(file);
                    JObject json = JObject.Parse(file);
                    GeneralInfo = JsonConvert.DeserializeObject<InfoModel>(json.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            info = GeneralInfo;
        }
        public InfoModel GetInfo()
        {
            if(info != null)
            {
                return info;
            }
            SetInfo();
            return info;
        }
        public List<DayOpenModel> GetHours()
        {
            try
            {
                using (StreamReader r = new StreamReader("/wwwroot/json/info.json"))
                {
                    string file = r.ReadToEnd();
                    JObject json = JObject.Parse(file);
                    InfoModel info = JsonConvert.DeserializeObject<InfoModel>(json.ToString());
                    return info.BusinessHours;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<DayOpenModel>();
            }
        }

        public bool UpdateInfo(InfoModel info)
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();
            string filePath = Path.Combine(rootDir, "/root/json/info.json");

            try
            {
                string json = JsonConvert.SerializeObject(info);
                Debug.WriteLine(json);
                File.WriteAllText(rootDir + "/wwwroot/json/info.json", json);
                SetInfo();
                return true;
            }catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        public bool AddHours(DayOpenModel hours) 
        {
            string id = System.Guid.NewGuid().ToString();
            hours.Id = id;
            info.BusinessHours.Add(hours);
            return UpdateInfo(info);
        }

        public bool UpdateHours(DayOpenModel hours)
        {
            bool updated = false;

            foreach(DayOpenModel day in info.BusinessHours)
            {
                if(day.Id ==  hours.Id) 
                {
                    day.ChangeHours(hours);
                    if (UpdateInfo(info))
                    {
                        SetInfo();
                        updated = true;
                    }
                }
            }
            return updated;
        }
    }
}
