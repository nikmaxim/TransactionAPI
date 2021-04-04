using MediatR;
using TransactionAPI.Models.JsonResponses;

namespace TransactionAPI.Requests
{
    public class DeleteListRequest : IRequest<ResponseJson>
    {
        public int TransactionId { get; set; }

        public DeleteListRequest(int transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
