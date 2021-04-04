using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class Import : Controller
    {
        private IMediator _mediator;

        public Import(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Uploads data from a .csv file to the server and merges with data in the database
        /// </summary>
        /// <param name="uploadedFile">Receives file and sends to the server</param>
        /// <returns>Returns status code with query result as JSON data</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Invalid data in the file</response>
        /// <response code="401">User is not authenticated</response> 
        /// <response code="415">Invalid file extension</response>
        /// <response code="500">Unexpected runtime error</response> 
        [HttpPost]
        [ProducesResponseType(typeof(Description), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Description), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Description), StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(typeof(Description), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(IFormFile uploadedFile)
        { 
            var request = new ImportRequest(uploadedFile, User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var result = await _mediator.Send(request);
            return StatusCode(result.Code, result.Description);
        }
    }
}
