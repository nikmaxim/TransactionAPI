using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using TransactionAPI.Requests;
using Microsoft.AspNetCore.Http;

namespace TransactionAPI.Controllers
{
    [Produces("text/plain")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Export : ControllerBase
    {
        private IMediator _mediator;

        public Export(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Downloads the .xlsx file to the user. You can use the filters like "Type" and "Status" to get the necessary data
        /// </summary>
        /// <param name="status">Status value that is considered for filter</param>
        /// <param name="type">Type value that is considered for filter</param>
        /// <returns>
        /// If the request is successful, it returns a .xlsx file with transaction data. 
        /// Otherwise, it returns an error message
        /// </returns>
        /// <response code="200">Successful operation</response>
        /// <response code="401">User is not authenticated</response> 
        /// <response code="404">Data not found</response> 
        [HttpGet]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string status, string type)
        {
            var request = new ExportRequest(status, type);
            var result = await _mediator.Send(request);
            return result != null ? File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "transactions.xlsx")
                : NotFound("{ \"error\":1,\"content\":\"Unable to export data from database!\" }");
        }
    }
}
