using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TransactionAPI.Models;
using TransactionAPI.Models.Database;
using TransactionAPI.Models.JsonResponses;
using TransactionAPI.Requests;
using TransactionAPI.Services;

namespace TransactionAPI.Handlers
{
    public class ImportHandler : IRequestHandler<ImportRequest, ResponseJson>
    {
        private ApplicationContext _db;

        public ImportHandler(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<ResponseJson> Handle(ImportRequest request, CancellationToken cancellationToken)
        {
            if (request.UploadedFile != null)
                if (request.UploadedFile.FileName.EndsWith(".csv"))                                  //Processing only .csv files
                {
                    using (StreamReader reader = new StreamReader(request.UploadedFile.OpenReadStream()))
                    {
                        string[] headers = reader.ReadLine().Split(',');                             //Skipping the first line with the columns titles
                        List<TransactionTemp> importedRows = new List<TransactionTemp>();

                        try
                        {
                            while (!reader.EndOfStream)
                            {
                                string[] tempRow = reader.ReadLine().Split(",");
                                importedRows.Add(new TransactionTemp
                                {
                                    Id = int.Parse(tempRow[0]),
                                    Status = tempRow[1],
                                    Type = tempRow[2],
                                    ClientName = tempRow[3],
                                    Amount = double.Parse(tempRow[4].Substring(1), System.Globalization.CultureInfo.InvariantCulture),
                                    UserId = request.UserId
                                });
                            }
                            importedRows = importedRows.DistinctBy(t => t.Id).Reverse().ToList();   //Removing the duplicate lines
                            await _db.TransactionsTemp.AddRangeAsync(importedRows);                 //Adds the imported data to the TransactionsTemp
                            await _db.SaveChangesAsync();

                            int mergeQuery = await _db.Database.ExecuteSqlRawAsync("" +             //SQL data merge query
                                "MERGE dbo.Transactions AS T_Base " +
                                "USING DBO.TransactionsTemp AS T_Source " +
                                "ON(T_Base.Id = T_Source.Id) " +
                                "WHEN MATCHED THEN " +
                                    "UPDATE SET Status = T_Source.Status " +
                                "WHEN NOT MATCHED THEN " +
                                    "INSERT(Id, Status, Type, ClientName, Amount, UserId) " +
                                    "VALUES(T_Source.Id, T_Source.Status, T_Source.Type, T_Source.ClientName, T_Source.Amount, T_Source.UserId);");
                        }
                        catch (FormatException)
                        {
                            return new ResponseJson
                            {
                                Code = 400,
                                Description = new Description
                                {
                                    Error = 1,
                                    Details = new List<Details>
                                {
                                    new Details
                                    {
                                        Name = "IncorrectType",
                                        Description = "Invalid data type to upload to the server!"
                                    }
                                },
                                    Content = "Upload to server failed!"
                                }
                            };
                        }
                        catch (OverflowException)
                        {
                            return new ResponseJson
                            {
                                Code = 400,
                                Description = new Description
                                {
                                    Error = 1,
                                    Details = new List<Details>
                                {
                                    new Details
                                    {
                                        Name = "OverflowData",
                                        Description = "The value is larger than the server allowed!"
                                    }
                                },
                                    Content = "Upload to server failed!"
                                }
                            };
                        }
                        catch (Exception)
                        {
                            return new ResponseJson
                            {
                                Code = 500,
                                Description = new Description
                                {
                                    Error = 1,
                                    Details = new List<Details>
                                {
                                    new Details
                                    {
                                        Name = "InternalServerError",
                                        Description = "An unexpected runtime error has occurred!"
                                    }
                                },
                                    Content = "Upload to server failed!"
                                }
                            };
                        }
                        finally //Clearing a table for temporary storage of transactions
                        {
                            await _db.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [TransactionsTemp];");
                            await _db.SaveChangesAsync();
                        }
                    }

                    return new ResponseJson
                    {
                        Code = 200,
                        Description = new Description
                        {
                            Error = 0,
                            Content = "Upload to server successful!"
                        }
                    };
                }
            return new ResponseJson
            {
                Code = 415,
                Description = new Description
                {
                    Error = 1,
                    Details = new List<Details>
                    {
                        new Details
                        {
                            Name = "IncorrectFileExtension",
                            Description = "The file must be with the .csv extension!"
                        }
                    },
                    Content = "Upload to server failed!"
                }
            };
        }
    }
}
