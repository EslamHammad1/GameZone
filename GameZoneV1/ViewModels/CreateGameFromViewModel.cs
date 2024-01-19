namespace GameZoneV1.ViewModels
{
    public class CreateGameFromViewModel :GameFormViewModel
    {
      
        [AllowedExtensions(FileSetting.AllowedExtensions),
              MaxFileSize(FileSetting.MaxFileSizeInBytes)]  
        public IFormFile Cover { get; set; } = default!;
    }
}
