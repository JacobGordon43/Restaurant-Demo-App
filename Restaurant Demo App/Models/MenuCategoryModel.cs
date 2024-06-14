namespace Restaurant_Demo_App.Models
{
    public class MenuCategoryModel
    {
        public string Category { get; set; }
        public List<MenuItemModel> Items { get; set; }

        public MenuCategoryModel(string category, List<MenuItemModel> items)
        {
            Category = category;
            Items = items;
        }
    }
}
