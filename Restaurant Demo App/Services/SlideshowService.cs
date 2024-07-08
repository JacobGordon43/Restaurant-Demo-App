using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restaurant_Demo_App.Models;
using System.Diagnostics;

namespace Restaurant_Demo_App.Services
{
    public class SlideshowService
    {
        public static List<ImageModel> Slideshow;
        public SlideshowService() { }

        public List<ImageModel> GetSlideshow()
        {
            if(Slideshow == null || Slideshow.Count == 0)
            {
                SetSlideshow();
            }
            return Slideshow;
        }
        public bool UploadImage(ImageModel image)
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();
            try
            {
                string id = Guid.NewGuid().ToString();
                using (FileStream stream = new FileStream(rootDir + "/wwwroot/images/" + id + ".png", FileMode.Create))
                {
                    image.UploadedFile.CopyTo(stream);
                }
                image.Source = "/images/" + id + ".png";
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public void SetSlideshow()
        {
            try
            {
                using (StreamReader r = new StreamReader("wwwroot/json/slideshow_images.json"))
                {
                    Slideshow = new List<ImageModel>();
                    string imagesString = r.ReadToEnd();
                    //JObject imagesJson = JObject.Parse(imagesString);
                    Slideshow = JsonConvert.DeserializeObject<List<ImageModel>>(imagesString);
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
            foreach (ImageModel imageModel in Slideshow)
            {
                if (imageModel.Id == image.Id) 
                {
                   
                    if (image.UploadedFile != null)
                    {
                        if (!UploadImage(image))
                        {
                            return false;
                        }
                    }
                    //Deletes the original image if the incoming change has a different source.
                    if (imageModel.Source != image.Source)
                    {
                        if (File.Exists(rootDir + "/wwwroot/images/" + imageModel.Source))
                        {
                            File.Delete(rootDir + "/wwwroot/images/" + imageModel.Source);
                        }
                    }
                    imageModel.ChangeData(image);
                }
            }
            return UpdateSlideshow();
        }
        public bool UpdateSlideshow() 
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();

            bool updated = false;
            try
            {
                string json = JsonConvert.SerializeObject(Slideshow);
                json.Replace("[", "{");
                json.Replace("]", "}");
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
            return UpdateSlideshow();
        }
    }
}
