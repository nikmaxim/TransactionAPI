using MediatR;

namespace TransactionAPI.Requests
{
    public class ExportRequest : IRequest<byte[]>
    {
        public string Status { get; set; }
        public string Type { get; set; }
       
        public ExportRequest(string status, string type)
        {
            Status = status;
            Type = type;            
        }
    }
}
