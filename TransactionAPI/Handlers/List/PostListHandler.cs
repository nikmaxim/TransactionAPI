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
    public class PostListHandler : IRequestHandler<PostListRequest, ResponseJson>
    {
        private ApplicationContext _db;
        public PostListHandler(ApplicationContext db)
        {
            _db = db;
        }
        public async Task<ResponseJson> Handle(PostListRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.CurrentStatus))
                return new ResponseJson
                {
                    Code = 422,
                    Description = new Description
                    {
                        Error = 1,
                        Details = new List<Details>
                        {
                            new Details
                            {
                                Name = "EmptyForm",
                                Description = "New status cannot be empty!"
                            }
                        },
                        Content = "Update failed!"
                    }
                };

            ResponseJson responseJson;
            Transaction transaction = await _db.Transactions.FirstOrDefaultAsync(i => i.Id == request.TransactionId);
            if (transaction != null)
            {
                transaction.Status = request.CurrentStatus;
                _db.Transactions.Update(transaction);
                await _db.SaveChangesAsync();
                responseJson = new ResponseJson
                {
                    Code = 200,
                    Description = new Description
                    {
                        Error = 0,
                        Content = "Update completed successfully!"
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
                        Content = "Update failed!"
                    }
                };
            }
            return responseJson;
        }
    }
}
