namespace GameZoneV1.Services
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public GamesService(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSetting.ImagesPath}";
        }
        public IEnumerable<Game> GetAll()
        {
            return _context.Games.
                Include(g => g.Category).
                Include(g => g.Devices).
                ThenInclude(g => g.Device).
                AsNoTracking()
                .ToList();
        }
        public Game? GetById(int id)
        {
            return _context.Games.
                Include(g => g.Category).
                Include(g => g.Devices).
                ThenInclude(g => g.Device).
                AsNoTracking().
                SingleOrDefault(g => g.Id == id);
        }
        public async Task Create(CreateGameFromViewModel gameModel)
        {
            var coverName = await SaveCover(gameModel.Cover);

            Game game = new()
            {
                Name = gameModel.Name,
                Description = gameModel.Description,
                Cover = coverName,
                CategoryId = gameModel.CategoryId,
                Devices = gameModel.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
            };
            _context.Add(game);
            _context.SaveChanges();
        }

        public async Task<Game?> Edit(EditViewModel model)
        {
            var game = _context.Games
                .Include(g=>g.Devices)
                .SingleOrDefault(g=>g.Id == model.Id);
            if (game == null)
                return null;

            var hasNewCover = model.Cover != null;
            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();
            if (hasNewCover)
            {
                game.Cover = await SaveCover(model.Cover!);
            }
            var effectedRows = _context.SaveChanges();
            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover); 
                return null;
            }

        }
        public bool Delete(int id)
        {
            var isDeleted = false;
            var game = _context.Games.Find(id);
            if (game == null)
                return isDeleted;
            _context.Remove(game);
           var effectedRows= _context.SaveChanges();
            if (effectedRows > 0) 
            {
            isDeleted = true;
                var Cover = Path.Combine(_imagesPath, game.Cover);
                 File.Delete(Cover);
            }
            return isDeleted;
        }
        private async Task<string> SaveCover(IFormFile Cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(Cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);
            using var stream = File.Create(path);
            await Cover.CopyToAsync(stream);
            return coverName;
        }

       
    }
}