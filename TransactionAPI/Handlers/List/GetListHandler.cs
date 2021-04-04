using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Models;
using TransactionAPI.Models.Database;
using TransactionAPI.Requests;

namespace TransactionAPI.Handlers
{
    public class GetListHandler : IRequestHandler<GetListRequest, List<Transaction>>
    {
        private ApplicationContext _db;
        public GetListHandler(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<List<Transaction>> Handle(GetListRequest request, CancellationToken cancellationToken)
        {
            IQueryable<Transaction> transactions = _db.Transactions;
            if (!string.IsNullOrEmpty(request.Type))                                                //Filter by type
                transactions = transactions.Where(i => i.Type.Contains(request.Type));
            if (!string.IsNullOrEmpty(request.Status))                                              //Filter by status
                transactions = transactions.Where(i => i.Status.Contains(request.Status));
            if (!string.IsNullOrEmpty(request.ClientName))                                          //Filter by client name
                transactions = transactions.Where(i => i.ClientName.Contains(request.ClientName));

            return await transactions.ToListAsync();                                                //Final response
        }
    }
}
