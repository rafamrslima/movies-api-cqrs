using System;
using ApiApplication.Database.Entities;
using MediatR;

namespace ApiApplication.Commands.BuySeats
{
	public class BuySeatsCommand : IRequest<TicketEntity>
    {
		public Guid ReservationId { get; set; }
	}
}

