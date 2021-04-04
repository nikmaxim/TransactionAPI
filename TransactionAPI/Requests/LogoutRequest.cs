using MediatR;
using TransactionAPI.Models.JsonResponses;

namespace TransactionAPI.Requests
{
    public class LogoutRequest : IRequest<ResponseJson>
    {
    }
}
