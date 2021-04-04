using MediatR;
using System.Collections.Generic;
using TransactionAPI.Models.Database;

namespace TransactionAPI.Requests
{
    public class GetListRequest : IRequest<List<Transaction>>
    {
        public string Status { get; set; }
        public string Type { get; set; }
        public string ClientName { get; set; }

        public GetListRequest(string status, string type, string clientName)
        {
            Status = status;
            Type = type;
            ClientName = clientName;
        }
    }
}
