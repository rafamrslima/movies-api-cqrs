using System;
using System.Threading;
using System.Threading.Tasks;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using MediatR;

namespace ApiApplication.Commands.BuySeats
{
	public class BuySeatsCommandHandler : IRequestHandler<BuySeatsCommand, TicketEntity>
    {
        private readonly ITicketsRepository _ticketsRepository;

        public BuySeatsCommandHandler(ITicketsRepository ticketsRepository)
		{
            _ticketsRepository = ticketsRepository;
        }

        public async Task<TicketEntity> Handle(BuySeatsCommand request, CancellationToken cancellationToken)
        {
            var ticketEntity = await _ticketsRepository.GetAsync(request.ReservationId, new CancellationToken());

            if(ticketEntity == null)
            {
                throw new ArgumentException("Reservation not found.");
            }

            if (ticketEntity.Paid)
            {
                throw new ArgumentException("Ticket is already paid.");
            }

            if (ticketEntity.CreatedTime < DateTime.Now.AddMinutes(-10))
            {
                throw new ArgumentException("Reservation is expired");
            }

            return await _ticketsRepository.ConfirmPaymentAsync(ticketEntity, new CancellationToken());
        }
    }
}

