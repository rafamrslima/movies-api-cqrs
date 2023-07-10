using System.Threading.Tasks;
using ApiApplication.Commands.CreateShowtime;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ShowtimesController : ControllerBase
	{
        private readonly IMediator _mediator;

		public ShowtimesController(IMediator mediator)
		{
            _mediator = mediator;
        }

        [HttpPost("create")]
		public async Task<IActionResult> CreateShowtime(CreateShowtimeCommand createShowtimeCommand)
		{
            await _mediator.Send(createShowtimeCommand);
            return StatusCode(201);
		} 
    }
}

