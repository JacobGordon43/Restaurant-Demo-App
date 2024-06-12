namespace Restaurant_Demo_App.Models
{
    public class MenuItemModel
    {
        public string name;
        public string description { get; set; }
        public float price;
        public int calories { get; set; }
        public bool peanuts { get; set; }
        public bool gluten { get; set; }
        public bool vegan { get; set; }

        public MenuItemModel(string name, string description, float price, int calories, bool peanuts, bool gluten, bool vegan)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.calories = calories;
            this.peanuts = peanuts;
            this.gluten = gluten;
            this.vegan = vegan;
        }
    }
}
