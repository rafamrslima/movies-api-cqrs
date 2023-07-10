using System;
namespace ApiApplication.Commands.ReserveSeats
{
	public class ReserveSeatsCommandResult
	{
		public Guid ReservationId { get; set; }
		public int SeatsTotal { get; set; }
		public int AuditoriumId { get; set; }
		public string MovieTitle { get; set; }
	}
}

