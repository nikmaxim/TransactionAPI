using MediatR;
using Microsoft.AspNetCore.Http;
using TransactionAPI.Models.JsonResponses;

namespace TransactionAPI.Requests
{
    public class ImportRequest : IRequest<ResponseJson>
    {
        public IFormFile UploadedFile { get; set; }
        public string UserId { get; set; }

        public ImportRequest(IFormFile uploadedFile, string userId)
        {
            UploadedFile = uploadedFile;
            UserId = userId;
        }
    }
}
