namespace GameZoneV1.Services
{
    public interface IGamesService
    {
        IEnumerable<Game> GetAll();
        Game? GetById(int id);
        Task Create(CreateGameFromViewModel gameModel);
        Task<Game?> Edit(EditViewModel model);
        bool Delete(int id);
    }
}