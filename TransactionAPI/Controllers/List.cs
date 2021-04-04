using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TransactionAPI.Models.Database;
using TransactionAPI.Models.JsonResponses;
using TransactionAPI.Requests;

namespace TransactionAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class List : ControllerBase
    {
        private IMediator _mediator;

        public List(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// This method allows you to get a list of transactions in JSON format from the database
        /// Possible to use filters like "Type", "Status" or "Client name"
        /// </summary>
        /// <param name="status">Status of transaction</param>
        /// <param name="type">Type of transaction</param>
        /// <param name="clientName">All clients with the same name</param>
        /// <returns>Returns JSON list with transactions list or 404 code if the list is empty</returns> 
        /// <response code="200">Successful operation</response>
        /// <response code="401">User is not authenticated</response> 
        /// <response code="404">Data not found</response> 
        [HttpGet]
        [ProducesResponseType(typeof(Transaction), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Array), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string status, string type, string clientName)
        {
            var request = new GetListRequest(status, type, clientName);
            var result = await _mediator.Send(request);
            return result != null ? Ok(result) : NotFound();
        }
        /// <summary>
        /// This method allows you to update the status of transaction in the database 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /List/{transactionId:int}
        ///     
        ///         "Completed"
        /// 
        /// </remarks>
        /// <param name="transactionId">The Id of transaction to be changed</param>
        /// <param name="currentStatus">New transaction status</param>
        /// <returns>Returns status code with a message about the result of implementation</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Does not match validation</response> 
        /// <response code="401">User is not authenticated</response> 
        /// <response code="404">Data not found</response> 
        /// <response code="422">Body parameter is empty</response> 
        [HttpPost("{transactionId}")]
        [ProducesResponseType(typeof(Description), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Description), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Description), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post(int transactionId, [FromBody] string currentStatus)
        {
            var request = new PostListRequest(transactionId, currentStatus);
            var result = await _mediator.Send(request);
            return StatusCode(result.Code, result.Description);            
        }

        /// <summary>
        /// This method allows you to delete a transaction from the database
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /List/{transactionId:int}
        /// 
        /// </remarks>
        /// <param name="transactionId">The Id of the transaction to be deleted</param>
        /// <returns>Returns status code with a message about the result of implementation</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Does not match validation</response> 
        /// <response code="401">User is not authenticated</response> 
        /// <response code="404">Data not found</response> 
        [HttpDelete("{transactionId}")]
        [ProducesResponseType(typeof(Description), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Description), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int transactionId)
        {
            var request = new DeleteListRequest(transactionId);
            var result = await _mediator.Send(request);
            return StatusCode(result.Code, result.Description);
        }
    }
}
