namespace Restaurant_Demo_App.Models
{
    public class ImageModel
    {
       public string Source { get; set; }
       public string Caption { get; set; }

        public ImageModel(string source, string caption)
        {
            Source = source;
            Caption = caption;
        }
    }
}
