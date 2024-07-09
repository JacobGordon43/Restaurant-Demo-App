using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public ImageModel UploadImage(ImageModel image)
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
                return null;
            }
            return image;
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
                   //Checks if a file was uploaded and runs the UploadedImage method to upload it, then checks to see if that was done successfully
                    if (image.UploadedFile != null)
                    {
                        image = UploadImage(image);
                        if (image == null)
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

        public bool DeleteImage(ImageModel image)
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();

            bool deleted = false;
            for(int i = 0; i < Slideshow.Count; i++)
            {
                //Removes the image from the slideshow
                if (Slideshow[i].Id == image.Id)
                {
                    try
                    {
                        if (File.Exists(rootDir + "/wwwroot/images/" + image.Source))
                        {
                            File.Delete(rootDir + "/wwwroot/images/" + image.Source);
                        }
                    }
                    catch(Exception ex) 
                    { 
                        Debug.WriteLine(ex); 
                    }
                    Slideshow.RemoveAt(i);
                    //Attempts to save the Slideshow to the file, if it fails it will reset the Slideshow value to the original Slideshow that is stored in the json file.
                    if (UpdateSlideshow())
                    {
                        deleted = true;
                    }
                    else
                    {
                        SetSlideshow();
                    }
                }
            }
            return deleted;
        }
        
        public bool UpdateSlideshow() 
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
            image = UploadImage(image);
            //Failure to upload the image will result in a false return statement. The image will not be added to the slideshow.
            if(image == null)
            {
                return false;
            }
            Slideshow.Add(image);
            return UpdateSlideshow();
        }
    }
}
