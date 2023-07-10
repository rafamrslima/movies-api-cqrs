using ApiApplication.Database.Entities;
using System.Collections.Generic;
using MediatR;

namespace ApiApplication.Commands.ReserveSeats
{
	public class ReserveSeatsCommand: IRequest<ReserveSeatsCommandResult>
	{
        public int ShowtimeId { get; set; }
        public ICollection<SeatEntity> Seats { get; set; }
    }
}

