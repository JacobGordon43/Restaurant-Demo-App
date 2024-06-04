namespace Restaurant_Demo_App.Models
{
    public class MenuCategoryModel
    {
        public string category { get; set; }
        public List<MenuItemModel> items { get; set; }

        public MenuCategoryModel(string category, List<MenuItemModel> items)
        {
            this.category = category;
            this.items = items;
        }
    }
}
