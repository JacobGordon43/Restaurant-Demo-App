namespace Restaurant_Demo_App.Models
{
    public class MenuModel
    {
        public List<MenuCategoryModel> Categories { get; set; }
        public List<List<MenuItemModel>> MenuItems { get; set; }

        public MenuModel(List<MenuCategoryModel> categories, List<List<MenuItemModel>> menuItems)
        {
            Categories = categories;
            MenuItems = menuItems;
        }
    }
}
