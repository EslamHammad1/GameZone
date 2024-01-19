namespace GameZoneV1.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly ApplicationDBContext _context;
        public DevicesService( ApplicationDBContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetSelectItems()
        {
            return _context.Devices.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                 .OrderBy(c => c.Text)
                 .AsNoTracking()
                 .ToList();
        }
    }
}
