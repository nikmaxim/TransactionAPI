using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransactionAPI.Models.JsonResponses;
using TransactionAPI.Requests;

namespace TransactionAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Logout : ControllerBase
    {
        private IMediator _mediator;

        public Logout(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// User logout process
        /// </summary>
        /// <returns>Returns status code with a message about the result of implementation.</returns>
        /// <response code="200">Successful logout</response>
        [HttpPost]
        [ProducesResponseType(typeof(Description), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post()
        {
            var request = new LogoutRequest();
            var result = await _mediator.Send(request);
            return StatusCode(result.Code, result.Description);
        }
    }
}
