namespace GameZoneV1.ViewModels
{
    public class EditViewModel:GameFormViewModel
    {
        public  int Id { get; set; }
        public string? CurrentCover { get; set; }
        [AllowedExtensions(FileSetting.AllowedExtensions),
           MaxFileSize(FileSetting.MaxFileSizeInBytes)]
        public IFormFile? Cover { get; set; } = default!;

    }
}
