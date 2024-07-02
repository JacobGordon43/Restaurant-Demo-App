using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restaurant_Demo_App.Models;
using System.Diagnostics;

namespace Restaurant_Demo_App.Services
{
    public class SlideshowService
    {
        public List<ImageModel> Slideshow;
        public SlideshowService() { }

        public List<ImageModel> GetSlideshow()
        {
            if(Slideshow == null || Slideshow.Count == 0)
            {
                SetSlideshow();
            }
            return Slideshow;
        }

        public void SetSlideshow()
        {
            try
            {
                using (StreamReader r = new StreamReader("wwwroot/json/slideshow_images.json"))
                {
                    Slideshow = new List<ImageModel>();
                    string imagesString = r.ReadToEnd();
                    JObject imagesJson = JObject.Parse(imagesString);
                    Debug.WriteLine(imagesJson.ToString());
                    foreach (JObject imageJson in imagesJson.Values())
                    {
                        ImageModel image = JsonConvert.DeserializeObject<ImageModel>(imageJson.ToString());
                        Slideshow.Add(image);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public bool UpdateImage(ImageModel image)
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();
            bool updated = false;
            foreach (ImageModel imageModel in Slideshow)
            {
                if (imageModel.Id == image.Id) 
                {
                    imageModel.ChangeData(image);
                    if(imageModel.Source != image.Source)
                    {
                        if(File.Exists(rootDir + "/wwwroot/images/" + imageModel.Source))
                        {
                            File.Delete(rootDir + "/wwwroot/images/" + imageModel.Source);
                        }
                    }
                    updated = true;
                }
            }
            return updated;
        }
        public bool UpdateSlideshow(ImageModel image) 
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();

            bool updated = false;
            try
            {
                string json = JsonConvert.SerializeObject(Slideshow);

                File.WriteAllText(rootDir + "/wwwroot/json/slideshow_images.json", json);
                updated = true;
            }catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return updated; 
        }

        public bool AddToSlideshow(ImageModel image)
        {
            string id = System.Guid.NewGuid().ToString();
            image.Id = id;
            Slideshow.Add(image);
            return UpdateSlideshow(image);
        }
    }
}
