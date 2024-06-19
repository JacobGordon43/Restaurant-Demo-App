using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Restaurant_Demo_App.Models;
using System.Diagnostics;

namespace Restaurant_Demo_App.Services
{
    public class MenuService
    {
        public static List<MenuCategoryModel> Menu;
        
        public MenuService() 
        {
            Menu = GetMenu();
        }
        /// <summary>
        /// Obtains the Menu and sets it's values
        /// </summary>
        public void SetMenu()
        {
            List<MenuCategoryModel> categories = new List<MenuCategoryModel>();
            try
            {
                using (StreamReader r = new StreamReader("wwwroot/json/menu.json"))
                {
                    string menuCategories = r.ReadToEnd();
                    JObject menuCategoriesJson = JObject.Parse(menuCategories);
                    Debug.WriteLine(menuCategoriesJson.ToString());
                    foreach (JObject menuCategoryJson in menuCategoriesJson["Menu"].Values())
                    {
                        Debug.WriteLine(menuCategoriesJson.ToString());
                        Debug.WriteLine(menuCategoryJson.ToString());
                        MenuCategoryModel category = JsonConvert.DeserializeObject<MenuCategoryModel>(menuCategoryJson.ToString());
                        Debug.WriteLine("The category is " + category.Category);
                        categories.Add(category);
                        Menu = categories;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public List<string> GetCategories()
        {
            List<string> categories = new List<string>();
            try
            {
                using (StreamReader r = new StreamReader("wwwroot/json/menu.json"))
                {
                    string menuCategories = r.ReadToEnd();
                    JObject menuCategoriesJson = JObject.Parse(menuCategories);
                    Debug.WriteLine(menuCategoriesJson.ToString());
                    foreach (JObject menuCategoryJson in menuCategoriesJson["Menu"].Values())
                    {
                        categories.Add(menuCategoryJson["Category"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return categories;
        }
        /// <summary>
        /// Obtains the Menu, sets it if it hasn't been already
        /// </summary>
        /// <returns>The entire menu, consisting of each category with all of it's items</returns>
        public List<MenuCategoryModel> GetMenu()
        {
            if (Menu == null)
            {
                SetMenu();
            }
            return Menu;
        }
        /// <summary>
        /// Updates an item by finding the corresponding category and item ID
        /// </summary>
        /// <param name="item">The Menu Item that is being updating</param>
        /// <returns>A boolean value to indicate whether it was updated or not</returns>
        public bool UpdateMenu(MenuItemModel item)
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();
            string filePath = Path.Combine(rootDir, "/root/json/menu.json");
            Debug.WriteLine(rootDir);
            try
            {
                foreach (MenuCategoryModel category in Menu)
                {
                    if (category.Category == item.Category)
                    {
                        category.Items.ForEach(dish =>
                        {
                            if (dish.Id == item.Id)
                            {
                                dish.ChangeItem(item.Name, item.Description, item.Price, item.Calories, item.Peanuts, item.Gluten, item.Vegan);
                            }
                        });
                    }
                }
                string menuCategoriesJson = "";
                int count = 0;
                foreach (MenuCategoryModel category in Menu)
                {

                    string json = JsonConvert.SerializeObject(category);
                    menuCategoriesJson += category.Category + ":" + json;
                    if (count < Menu.Count() - 1)
                    {
                        menuCategoriesJson += ",";
                    }
                    count++;
                }

                File.WriteAllText(rootDir + "/wwwroot/json/menu.json", "{'Menu':{" + menuCategoriesJson + "}}");
                SetMenu();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Deletes an item from the menu
        /// </summary>
        /// <param name="item">The item to be deleted</param>
        /// <returns>A boolean value based on whether or not the item was deleted</returns>
        public bool DeleteItem(MenuItemModel item)
        {
            string rootDir = @Directory.GetCurrentDirectory().ToString();
            string filePath = Path.Combine(rootDir, "/root/json/menu.json");
            try
            {
                foreach (MenuCategoryModel category in Menu)
                {
                    if (category.Category == item.Category)
                    {
                        for (int i = 0; i < category.Items.Count(); i++)
                        {
                            if (category.Items[i].Id == item.Id)
                            {
                                category.Items.RemoveAt(i);
                            }
                        }
                    }
                }
                string menuCategoriesJson = "";
                int count = 0;
                foreach (MenuCategoryModel category in Menu)
                {

                    string json = JsonConvert.SerializeObject(category);
                    menuCategoriesJson += category.Category + ":" + json;
                    if (count < Menu.Count() - 1)
                    {
                        menuCategoriesJson += ",";
                    }
                    count++;
                }

                System.IO.File.WriteAllText(rootDir + "/wwwroot/json/menu.json", "{'Menu':{" + menuCategoriesJson + "}}");
              
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
            return true;
        }

        public bool CreateItem(MenuItemModel item)
        {
            string id = System.Guid.NewGuid().ToString();
            item.Id = id;
            return true;
        }
    }
}
