using MediatR;
using TransactionAPI.Models.JsonResponses;

namespace TransactionAPI.Requests
{
    public class PostListRequest : IRequest<ResponseJson>
    {
        public int TransactionId { get; set; }
        public string CurrentStatus { get; set; }

        public PostListRequest(int transactionId, string currentStatus)
        {
            TransactionId = transactionId;
            CurrentStatus = currentStatus;
        }
    }
}
