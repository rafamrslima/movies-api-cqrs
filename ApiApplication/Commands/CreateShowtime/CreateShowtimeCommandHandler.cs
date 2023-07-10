using System;
using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Requests;
using MediatR;

namespace ApiApplication.Commands.CreateShowtime
{
    public class CreateShowtimeCommandHandler : IRequestHandler<CreateShowtimeCommand, ShowtimeEntity>
    {
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IMoviesRequest _moviesRequest;
        private readonly IMoviesRepository _moviesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        public CreateShowtimeCommandHandler(
            IShowtimesRepository showtimesRepository,
            IMoviesRequest moviesRequest,
            IMoviesRepository moviesRepository,
            IAuditoriumsRepository auditoriumsRepository)
        {
            _showtimesRepository = showtimesRepository;
            _moviesRequest = moviesRequest;
            _moviesRepository = moviesRepository;
            _auditoriumsRepository = auditoriumsRepository;
        }

        public async Task<ShowtimeEntity> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            if (request.SessionDate < DateTime.Now)
                throw new ArgumentException($"Session must be in a future date.");

            var movieEntity = await _moviesRepository.GetMovieById(request.MovieId, cancellationToken)
                ?? throw new ArgumentException($"Movie '{request.MovieId}' is not valid.");

            _ = await _auditoriumsRepository.GetAsync(request.AuditoriumId, cancellationToken)
                ?? throw new ArgumentException($"Auditorium '{request.AuditoriumId}' is not valid.");
             
            var showtime = new ShowtimeEntity()
            {
                AuditoriumId = request.AuditoriumId,
                Movie = movieEntity,
                SessionDate = request.SessionDate
            };

            return await _showtimesRepository.CreateShowtime(showtime, cancellationToken); 
        } 
    }
}

