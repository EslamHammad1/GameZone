namespace GameZoneV1.Controllers
{
   
    public class GamesController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGamesService _gamesService;
        public GamesController(ApplicationDBContext context, ICategoriesService categoriesService ,
            IDevicesService devicesService, IGamesService gamesService)
        {
            _context = context;
            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _gamesService = gamesService;
        }

        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
            if (game==null)
            { 
                return NotFound();
                }
                return View(game);
        }
        [HttpGet]
        public IActionResult Create() 
        {
            CreateGameFromViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectItems()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFromViewModel newModel) 
        {
            if (!ModelState.IsValid)
            {
                newModel.Categories = _categoriesService.GetSelectList();
                newModel.Devices = _devicesService.GetSelectItems();
               return View(newModel);
            }

             await _gamesService.Create(newModel);
            return RedirectToAction(nameof(Index));  
        }
        [HttpGet]
        public IActionResult Edit(int id) 
        {
            var game = _gamesService.GetById(id);
            if(game==null) 
            return NotFound();
            
            EditViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectItems(),
                Description = game.Description,
                CurrentCover = game.Cover,
            };
            return View(viewModel) ;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model) 
        {
        if (!ModelState.IsValid) 
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectItems();
                return View(model);
            }
            var game = await _gamesService.Edit(model);
            if(game==null) 
                return BadRequest();
            
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            var isDeleted = _gamesService.Delete(id);
            return isDeleted? Ok() : BadRequest();
        }
    }
}
