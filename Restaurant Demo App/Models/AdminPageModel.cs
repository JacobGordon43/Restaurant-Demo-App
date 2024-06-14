namespace Restaurant_Demo_App.Models
{
    public class AdminPageModel
    {
        public List<MenuItemModel> Menu { get; set; }
        public List<AboutContentModel> AboutContent { get; set; }

        public AdminPageModel(List<MenuItemModel> menu, List<AboutContentModel> aboutContent)
        {
            Menu = menu;
            AboutContent = aboutContent;
        }
    }
}
