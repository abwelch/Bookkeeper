using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Bookkeeper;
using Bookkeeper.Data;
using Bookkeeper.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Bookkeeper.Models
{

    public interface ITransactionUtils
    {
        bool ValidateTransaction(TransactionViewModel transaction);
        int CommitTransaction(TransactionViewModel transaction);
        TransactionSummaryViewModel RetrieveLastUserCommitSummary(int userID);
    }

    public class TransactionUtils : ITransactionUtils
    {
        private readonly BookkeeperContext dbContext;
        private readonly IDbConnection dbConnection;
        public TransactionUtils(BookkeeperContext _dbContext, IDbConnection _dbConnection)
        {
            dbContext = _dbContext;
            dbConnection = _dbConnection;
        }

        public bool ValidateTransaction(TransactionViewModel transaction)
        {
            decimal? debitTotal = 0;
            decimal? creditTotal = 0;
            foreach (JournalLineItemViewModel lineItem in transaction.PreviousEntries)
            {
                if (lineItem.DebitAmount != null)
                {
                    debitTotal += lineItem.DebitAmount;
                }
                else
                {
                    creditTotal += lineItem.CreditAmount;
                }
            }
            if (debitTotal == creditTotal && debitTotal != 0)
            {
                return true;
            }
            return false;
        }

        public int CommitTransaction(TransactionViewModel transaction)
        {
            int tranID = -1;
            decimal? total = 0;
            foreach (JournalLineItemViewModel lineItem in transaction.PreviousEntries)
            {
                total += (lineItem.DebitAmount != null) ? lineItem.DebitAmount : 0;
            }

            JournalTransaction transactionEntry = new JournalTransaction()
            {
                Memo = transaction.JournalHeader.Memo,
                RecordedDateTime = DateTime.Now.ToString(),
                TotalAmount = (decimal)total,
                UserID = transaction.UserID
            };
            try
            {
                dbContext.JournalTransactions.Add(transactionEntry);
                dbContext.SaveChanges();
            }
            catch
            {
                return -1;
            }
            tranID = transactionEntry.TransactionId;
            foreach (JournalLineItemViewModel lineItem in transaction.PreviousEntries)
            {
                JournalEntry entry = new JournalEntry()
                {
                    AccountName = lineItem.AccountName,
                    DebitBalance = AccountsInfo.DebitAccounts.Contains(lineItem.AccountName),
                    DebitAmount = lineItem.DebitAmount,
                    CreditAmount = lineItem.CreditAmount,
                    ParentTransactionId = transactionEntry.TransactionId
                };
                try
                {
                    dbContext.JournalEntries.Add(entry);
                }
                catch
                {
                    return -1;
                }
            }
            try
            {
                dbContext.SaveChanges();
            }
            catch
            {
                return -1;
            }
            return tranID;
        }

        public TransactionSummaryViewModel RetrieveLastUserCommitSummary(int tranID)
        {
            // Utilize dapper queries

            string retrieveHeader = $"SELECT * FROM [Recording].JournalTransactions WHERE TransactionID = {tranID}";
            var header = dbConnection.QueryFirstOrDefault<JournalTransaction>(retrieveHeader);
            if (header == null)
            {
                return null;
            }

            string retrieveLineItems = $"SELECT * FROM [Recording].JournalEntries WHERE ParentTransactionID = {header.TransactionId}";
            var lineItems = dbConnection.Query<JournalEntry>(retrieveLineItems);

            TransactionSummaryViewModel mostRecentCommit = new TransactionSummaryViewModel()
            {
                JournalHeader = header,
                LineItems = lineItems.ToList(),
            };
            return mostRecentCommit;
        }
    }
}
