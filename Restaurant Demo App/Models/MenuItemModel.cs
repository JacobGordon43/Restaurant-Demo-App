using System.ComponentModel.DataAnnotations;

namespace Restaurant_Demo_App.Models
{
    public class MenuItemModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Category {get; set;}
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }
        [Required]
        public int Calories { get; set; }
        [Required]
        public bool Peanuts { get; set; }
        [Required]
        public bool Gluten { get; set; }
        [Required]
        public bool Vegan { get; set; }

        public MenuItemModel(string id, string category, string name, string description, float price, int calories, bool peanuts, bool gluten, bool vegan)
        {
            Id = id;
            Category = category;
            Name = name;
            Description = description;
            Price = price;
            Calories = calories;
            Peanuts = peanuts;
            Gluten = gluten;
            Vegan = vegan;
        }
        public MenuItemModel() { }

        public void ChangeItem(string name, string description, float price, int calories, bool peanuts, bool gluten, bool vegan)
        {
            Name = name;
            Description = description;
            Price = price;
            Calories = calories;
            Peanuts = peanuts;
            Gluten = gluten;
            Vegan = vegan;
        }
    }
}
