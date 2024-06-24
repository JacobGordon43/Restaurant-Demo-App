using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restaurant_Demo_App.Models;
using System.Diagnostics;
using Ganss.Xss;
namespace Restaurant_Demo_App.Services
{
    public class AboutService
    {
        public static AboutContentModel AboutContent;

        public AboutService() 
        {
            AboutContent = GetAboutContent();
        }
        public AboutContentModel GetAboutContent()
        {
            if(AboutContent == null || AboutContent.Content == null)
            {
                SetAboutContent();
            }
            return AboutContent;
        }

        public void SetAboutContent()
        {
            AboutContentModel content = new AboutContentModel();
            try
            {
                using(StreamReader r = new StreamReader("wwwroot/json/about.json"))
                {
                    string fileContent = r.ReadToEnd();
                    JObject contentJson = JObject.Parse(fileContent);
                    content = JsonConvert.DeserializeObject<AboutContentModel>(contentJson.ToString());
                }
            }catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            AboutContent = content;
        }

        public bool UpdateAboutContent(AboutContentModel content)
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();

            try
            {
                var sanitizer = new HtmlSanitizer();
                content.Content = sanitizer.Sanitize(content.Content);
                string json = JsonConvert.SerializeObject(content);
                System.IO.File.WriteAllText(rootDir + "/wwwroot/json/about.json",  json);
            }catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }
    }
}
