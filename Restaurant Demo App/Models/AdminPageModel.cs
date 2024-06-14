namespace Restaurant_Demo_App.Models
{
    public class AdminPageModel
    {
        public List<MenuCategoryModel> Menu { get; set; }
        public List<AboutContentModel> AboutContent { get; set; }

        public AdminPageModel(List<MenuCategoryModel> menu, List<AboutContentModel> aboutContent)
        {
            Menu = menu;
            AboutContent = aboutContent;
        }
    }
}
