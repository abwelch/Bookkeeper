using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using Bookkeeper.Models;
using System.Data;
using Bookkeeper.Data;

namespace Bookkeeper.Models
{
    public interface IToolsUtils
    {
        public Task<bool> ImportExcelToDatabase(ImportExcel file, int userID);
    }

    public class ToolsUtils : IToolsUtils
    {
        private readonly BookkeeperContext dbContext;
        private readonly IDbConnection dbConnection;
        public ToolsUtils(BookkeeperContext _dbContext, IDbConnection _dbConnection)
        {
            dbContext = _dbContext;
            dbConnection = _dbConnection;
        }

        public async Task<bool> ImportExcelToDatabase(ImportExcel file, int userID)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                await file.ExcelFile.CopyToAsync(stream);

                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    int rowCount = worksheet.Dimension.Rows;
                    int blankRowCounter = 0;
                    bool onHeader = true;
                    ImportTransaction newTransaction = new ImportTransaction();
                    // Process excel file
                    for (int row = 3; row <= rowCount; row++)
                    {
                        if ((worksheet.Cells[row, 2].Value.ToString().Equals(String.Empty)) || (worksheet.Cells[row, 2].Value.ToString().Equals(null)))
                        {
                            // Commit transaction that was just finished being traversed
                            if (blankRowCounter == 0)
                            {
                                decimal total = 0;
                                foreach (JournalEntry lineItem in newTransaction.LineItems)
                                {
                                    total += Convert.ToDecimal(worksheet.Cells[row, 3].Value.ToString());
                                }
                                newTransaction.Header.TotalAmount = total;
                                dbContext.JournalTransactions.Add(newTransaction.Header);
                                dbContext.SaveChanges();
                                foreach (JournalEntry lineItem in newTransaction.LineItems)
                                {
                                    dbContext.JournalEntries.Add(lineItem);
                                }
                                dbContext.SaveChanges();
                            }
                            blankRowCounter++;
                            continue;
                        }
                        else
                        {
                            blankRowCounter = 0;
                        }

                        // End of file
                        if (blankRowCounter > 1)
                        {
                            break;
                        }

                        // If date value exists, at new transaction header
                        if (!(worksheet.Cells[row, 1].Value.ToString().Equals(String.Empty)) || !(worksheet.Cells[row, 1].Value.ToString().Equals(null)))
                        {
                            onHeader = true;
                            newTransaction = new ImportTransaction();
                        }

                        if (onHeader)
                        {
                            newTransaction.Header.RecordedDateTime = worksheet.Cells[row, 1].Value.ToString();
                            newTransaction.Header.Memo = worksheet.Cells[row, 2].Value.ToString();
                            newTransaction.Header.UserID = userID;
                            onHeader = false;

                        }
                        else
                        {
                            JournalEntry lineItem = new JournalEntry();
                            lineItem.AccountName = worksheet.Cells[row, 2].Value.ToString();
                            if (worksheet.Cells[row, 3].Value.ToString().Equals(string.Empty) || worksheet.Cells[row, 1].Value.ToString().Equals(null))
                            {
                                lineItem.DebitBalance = false;
                                lineItem.CreditAmount = Convert.ToDecimal(worksheet.Cells[row, 4].Value.ToString());
                            }
                            else
                            {
                                lineItem.DebitBalance = true;
                                lineItem.CreditAmount = Convert.ToDecimal(worksheet.Cells[row, 3].Value.ToString());
                            }
                            newTransaction.LineItems.Add(lineItem);
                        }
                    }
                }
            }
            return true;
        }

        public class ImportTransaction
        {
            public ImportTransaction()
            {
                Header = new JournalTransaction();
                LineItems = new List<JournalEntry>();
            }
            public JournalTransaction Header { get; set; }
            public List<JournalEntry> LineItems { get; set; }
        }
    }
}
