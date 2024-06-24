namespace Restaurant_Demo_App.Models
{
    public class AboutContentModel
    {
        public string Content { get; set; }
        public AboutContentModel(string content) {
            Content = content;
        }

        public AboutContentModel() { }
    }
}