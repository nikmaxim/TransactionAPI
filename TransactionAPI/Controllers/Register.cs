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
    [ApiController]
    [Route("api/[controller]")]
    public class Register : ControllerBase
    {
        private IMediator _mediator;

        public Register(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Method allows to create a new user in the system.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///         "email":"ivanov@gmail.com",
        ///         "userName":"ivanov",
        ///         "password":"PassW@rd123"
        ///     }
        ///     
        /// </remarks>
        /// <param name="model">Includes email, username and password which are needed for registration</param>
        /// <returns>Returns status code with a message about the result of implementation.</returns>
        /// <response code="200">Successful registration</response>
        /// <response code="400">Does not match validation</response> 
        /// <response code="422">Value are incorrect</response> 
        [HttpPost]
        [ProducesResponseType(typeof(Description), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Description), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post([FromBody] RegisterRequestJson model)
        {
            var request = new RegisterRequest(model);
            var result = await _mediator.Send(request);
            return StatusCode(result.Code, result.Description);
        }
    }
}
