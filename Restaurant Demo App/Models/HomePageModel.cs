namespace Restaurant_Demo_App.Models
{
    public class HomePageModel
    {
        public List<MenuCategoryModel> Menu { get; set; }
        public List<ImageModel> Images { get; set; }
        
        public HomePageModel(List<MenuCategoryModel> Menu, List<ImageModel> Images)
        {
            this.Menu = Menu;
            this.Images = Images;
        }
    }
}