using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TransactionAPI.Models.JsonRequest;
using TransactionAPI.Models.JsonResponses;
using TransactionAPI.Requests;

namespace TransactionAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        private IMediator _mediator;

        public Login(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// User authentication process in the system
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Login
        ///     {
        ///         "userName":"ivanov",
        ///         "password":"PassW@rd123"
        ///     }
        ///     
        /// </remarks>
        /// <param name="model">Includes username and password which are needed for authentication</param>
        /// <returns>Returns status code with a message about the result of implementation</returns>
        /// <response code="200">Successful authentication</response>
        /// <response code="400">Does not match validation</response> 
        /// <response code="401">Value are incorrect</response> 
        [HttpPost]
        [ProducesResponseType(typeof(Description), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Description), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post([FromBody]LoginRequestJson model)
        {
            var request = new LoginRequest(model);
            var result = await _mediator.Send(request);
            return StatusCode(result.Code, result.Description);
        }
    }
}
