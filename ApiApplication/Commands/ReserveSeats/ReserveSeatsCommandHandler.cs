using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;

using MediatR;
using ProtoDefinitions;

namespace ApiApplication.Commands.ReserveSeats
{
    public class ReserveSeatsCommandHandler : IRequestHandler<ReserveSeatsCommand, ReserveSeatsCommandResult>
    {
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        public ReserveSeatsCommandHandler(ITicketsRepository ticketsRepository, IShowtimesRepository showtimesRepository, IAuditoriumsRepository auditoriumsRepository)
        {
            _ticketsRepository = ticketsRepository;
            _showtimesRepository = showtimesRepository;
            _auditoriumsRepository = auditoriumsRepository;
        }

        public async Task<ReserveSeatsCommandResult> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
        {
            await VerifyRequestedSeats(request.Seats, cancellationToken);
            await VerifyIfTicketIsAvailable(request, cancellationToken);
             
            var showtimeEntity = await _showtimesRepository.GetWithMoviesByIdAsync(request.ShowtimeId, cancellationToken);

            if (showtimeEntity == null)
            {
                throw new ArgumentException("Showtime not found.");
            }

            var ticketEntity = await _ticketsRepository.CreateAsync(showtimeEntity, request.Seats, cancellationToken);

            return new ReserveSeatsCommandResult()
            {
                ReservationId = ticketEntity.Id,
                SeatsTotal = request.Seats.Count,
                AuditoriumId = ticketEntity.Showtime.AuditoriumId,
                MovieTitle = ticketEntity.Showtime.Movie.Title
            };
        }
         
        private async Task VerifyRequestedSeats(ICollection<SeatEntity> seats, CancellationToken cancellationToken)
        {
            if (!seats.Any())
            {
                throw new ArgumentException("Seats must be selected.");
            }

            var auditoriums = seats.Select(x => x.AuditoriumId).ToList();

            if (auditoriums.Distinct().Count() > 1)
                throw new ArgumentException("Seats must be in the same auditorium.");

            var auditorium = await _auditoriumsRepository.GetAsync(auditoriums.FirstOrDefault(), cancellationToken);

            if (auditorium == null)
                throw new ArgumentException($"Auditorium '{auditoriums.FirstOrDefault()} is not valid.");

            foreach (var selectedSeat in seats)
            {
                if (!auditorium.Seats.Any(x => x.Row == selectedSeat.Row && x.SeatNumber == selectedSeat.SeatNumber))
                    throw new ArgumentException($"Seat {selectedSeat.SeatNumber} is not valid.");
            }

            var orderedSeats = seats.OrderBy(x => x.Row).ThenBy(x => x.SeatNumber).ToArray();
            var isContiguous = orderedSeats.Skip(1).Zip(orderedSeats, (x, y) => (x.Row == y.Row) && (x.SeatNumber - y.SeatNumber == 1)).All(z => z);

            if (!isContiguous)
            {
                throw new ArgumentException("Seats should be contiguos.");
            }
        }

        private async Task VerifyIfTicketIsAvailable(ReserveSeatsCommand request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketsRepository.GetEnrichedAsync(request.ShowtimeId, cancellationToken);

            if (tickets.Any())
            {
                var recentReservedTickets = tickets.Where(x => x.CreatedTime > DateTime.Now.AddMinutes(-10));

                foreach (var ticket in recentReservedTickets)
                {
                    bool seatsReservedInTheLast10Mins = request.Seats
                        .Any(x => ticket.Seats.Any(y => x.SeatNumber == y.SeatNumber && x.Row == y.Row));

                    if (seatsReservedInTheLast10Mins)
                    {
                        throw new ArgumentException("Seat was reserved less than 10 minutes ago.");
                    }
                }

                var paidTickets = tickets.Where(x => x.Paid);

                foreach (var ticket in paidTickets)
                {
                    bool seatAlreadySold = request.Seats
                        .Any(x => ticket.Seats.Any(y => y.SeatNumber == x.SeatNumber && x.Row == y.Row));

                    if (seatAlreadySold)
                    {
                        throw new ArgumentException("Seat is already sold.");
                    }
                }
            }
        }
    }
}

