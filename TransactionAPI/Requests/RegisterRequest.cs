using MediatR;
using TransactionAPI.Models.JsonRequest;
using TransactionAPI.Models.JsonResponses;

namespace TransactionAPI.Requests
{
    public class RegisterRequest : IRequest<ResponseJson>
    {
        public RegisterRequestJson RegisterRequestJson { get; set; }
        
        public RegisterRequest(RegisterRequestJson registerRequestJson)
        {
            RegisterRequestJson = registerRequestJson;
        }
    }
}
