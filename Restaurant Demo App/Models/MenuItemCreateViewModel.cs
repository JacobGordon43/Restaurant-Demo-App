using Restaurant_Demo_App.Services;

namespace Restaurant_Demo_App.Models
{
    public class MenuItemCreateViewModel
    {
        public List<string> categories;
        public MenuItemModel item;

        public MenuItemCreateViewModel() 
        { 
            MenuService menuService = new MenuService();
            categories = menuService.GetCategories();
            item = new MenuItemModel();
        }
    }
}
