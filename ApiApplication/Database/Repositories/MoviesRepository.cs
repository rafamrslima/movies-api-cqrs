using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Database.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly CinemaContext _context;

        public MoviesRepository(CinemaContext context)
        {
            _context = context;
        }
        public async Task<MovieEntity> GetMovieById(int id, CancellationToken cancel)
        {
            return await _context.Movies.FirstOrDefaultAsync(x => x.Id == id, cancel);
        }
    }
}
