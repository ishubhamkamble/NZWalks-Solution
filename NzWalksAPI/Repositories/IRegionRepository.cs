using NzWalksAPI.Models.Domain;

namespace NzWalksAPI.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();


    }
}
