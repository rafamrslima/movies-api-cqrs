using System.Threading.Tasks;

namespace ApiApplication.Requests
{
	public interface IMoviesRequest
	{
        Task<MoviesApiResponse> GetMovieById(string id); 
    }
}

