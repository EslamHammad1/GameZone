namespace GameZoneV1.Services
{
    public interface ICategoriesService
    { 
        IEnumerable<SelectListItem> GetSelectList();
    }
}
