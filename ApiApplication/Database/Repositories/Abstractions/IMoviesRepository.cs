using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IMoviesRepository
    {
        Task<MovieEntity> GetMovieById(int id, CancellationToken cancel);
    }
}