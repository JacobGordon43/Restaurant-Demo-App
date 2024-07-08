namespace Restaurant_Demo_App.Models
{
    public class ImageModel
    {
       public string Id { get; set; }
       public string Source { get; set; }
        public string Caption { get; set; }
        public IFormFile UploadedFile { get; set; }

        public ImageModel(string id, string source, string caption)
        {
            Id = id;
            Source = source;
            Caption = caption;
        }
        public ImageModel() { }

        public void ChangeData(ImageModel imageModel) 
        { 
            Source = imageModel.Source;
            Caption = imageModel.Caption;
        }

    }
}
