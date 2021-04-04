using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Models;
using TransactionAPI.Models.Database;
using TransactionAPI.Requests;

namespace TransactionAPI.Handlers
{
    public class ExportHandler : IRequestHandler<ExportRequest, byte[]>
    {
        private ApplicationContext _db;
        public ExportHandler(ApplicationContext db)
        {
            _db = db;
        }
        public async Task<byte[]> Handle(ExportRequest request, CancellationToken cancellationToken)
        {
            IQueryable<Transaction> transactions = _db.Transactions;
            if (!string.IsNullOrEmpty(request.Type))                                        //Filter by type
                transactions = transactions.Where(i => i.Type.Contains(request.Type)); 
            if (!string.IsNullOrEmpty(request.Status))                                      //Filter by status
                transactions = transactions.Where(i => i.Status.Contains(request.Status));

            List<Transaction> transactionsList = await transactions.ToListAsync();
            if (transactionsList == null)
                return null;
            transactionsList.ForEach(i => i.AmountString = i.Amount.ToString("$0.00"));     //Formatting the amount to display correctly 
            var formattedTransactionsList = transactionsList.Select(p => new { p.Id, p.Status, p.Type, p.ClientName, p.AmountString, p.UserId }).ToList();

            XLWorkbook workBook = new XLWorkbook();
            workBook.AddWorksheet("Worksheet");
            IXLWorksheet workSheet = workBook.Worksheet("Worksheet");
            workSheet.Cell(1, 1).InsertData(new List<string> { "TransactionId", "Status", "Type", "ClientName", "Amount", "UserId" }, true);
            workSheet.Cell(2, 1).InsertData(formattedTransactionsList);                     //Data is added to the .xlsx document

            byte[] workbookBytes = null;
            using (MemoryStream memoryStream = new MemoryStream())                                   //Converting .xlsx to byte array to send correctly to client
            {
                workBook.SaveAs(memoryStream);
                workbookBytes = memoryStream.ToArray();
            }

            return workbookBytes;
        }
    }
}
