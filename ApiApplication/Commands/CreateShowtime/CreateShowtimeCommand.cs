using System;
using ApiApplication.Database.Entities;
using MediatR;

namespace ApiApplication.Commands.CreateShowtime
{
	public class CreateShowtimeCommand : IRequest<ShowtimeEntity>
	{ 
        public int MovieId { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
    }
}

