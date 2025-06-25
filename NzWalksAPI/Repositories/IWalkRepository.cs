using NzWalksAPI.Models.Domain;

namespace NzWalksAPI.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
    }
}
