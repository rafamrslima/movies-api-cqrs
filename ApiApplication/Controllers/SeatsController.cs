using System.Threading.Tasks;
using ApiApplication.Commands.BuySeats;
using ApiApplication.Commands.ReserveSeats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiApplication.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class SeatsController : ControllerBase
    {
         private readonly IMediator _mediator;

        public SeatsController(IMediator mediator)
        {
             _mediator = mediator;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveSeats(ReserveSeatsCommand reserveSeatsCommand)
        {
            var response = await _mediator.Send(reserveSeatsCommand);
            return Ok(response);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuySeats(BuySeatsCommand buySeatsCommand)
        {
            await _mediator.Send(buySeatsCommand);
            return Ok("Ticket paid successfully.");
        }
    } 
}
