using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Models;
using TransactionAPI.Models.Database;
using TransactionAPI.Models.JsonResponses;
using TransactionAPI.Requests;

namespace TransactionAPI.Handlers
{
    public class DeleteListHandler : IRequestHandler<DeleteListRequest, ResponseJson>
    {
        private ApplicationContext _db;
        public DeleteListHandler(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<ResponseJson> Handle(DeleteListRequest request, CancellationToken cancellationToken)
        {
            ResponseJson responseJson;
            Transaction transaction = await _db.Transactions.FirstOrDefaultAsync(i => i.Id == request.TransactionId);
            if (transaction != null)
            {
                _db.Transactions.Remove(transaction);
                await _db.SaveChangesAsync();
                responseJson = new ResponseJson
                {
                    Code = 200,
                    Description = new Description
                    {
                        Error = 0,
                        Content = "Deletion completed successfully!"
                    }
                };
            }
            else
            {
                responseJson = new ResponseJson
                {
                    Code = 404,
                    Description = new Description
                    {
                        Error = 1,
                        Details = new List<Details>
                        {
                            new Details
                            {
                                Name = "IncorrectForm",
                                Description = "No transaction with same Id!"
                            }
                        },
                        Content = "Deletion failed!"
                    }
                };
            }
            return responseJson;
        }
    }
}
