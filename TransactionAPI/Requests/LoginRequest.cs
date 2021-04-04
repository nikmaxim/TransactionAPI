using MediatR;
using TransactionAPI.Models.JsonRequest;
using TransactionAPI.Models.JsonResponses;

namespace TransactionAPI.Requests
{
    public class LoginRequest : IRequest<ResponseJson>
    {
        public LoginRequestJson LoginRequestJson { get; set; }

        public LoginRequest(LoginRequestJson loginRequestJson)
        {
            LoginRequestJson = loginRequestJson;
        }
    }
}
