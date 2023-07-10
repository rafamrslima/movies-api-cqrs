using System;
using System.Net.Http;
using System.Threading.Tasks; 
using Newtonsoft.Json;

namespace ApiApplication.Requests
{
	public class MoviesRequest : IMoviesRequest
    { 
		public async Task<MoviesApiResponse> GetMovieById(string id)
		{
            try
            {
                var url = $"https://localhost:7443/v1/movies/{id}";
                var key = "68e5fbda-9ec9-4858-97b2-4a8349764c63";

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using var client = new HttpClient(clientHandler);

                client.DefaultRequestHeaders.Add("X-Apikey", key);
                var content = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<MoviesApiResponse>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
	}
}

