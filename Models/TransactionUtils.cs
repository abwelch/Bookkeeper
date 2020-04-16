using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
        JournalViewModel RetrieveAllTransactionsByUser(int userID);
        TransactionViewModel ConvertToTransactionViewModel(int transactionID);
        void DeleteTransaction(int transactionID);
        Dictionary<string, LedgerAccount> Post(int userID);
    }

    public class TransactionUtils : ITransactionUtils
    {
        private readonly BookkeeperContext dbContext;
        private readonly IDbConnection dbConnection;
        private readonly IUserInfoUtils userInfoUtils;
        public TransactionUtils(BookkeeperContext _dbContext, IDbConnection _dbConnection, IUserInfoUtils _userInfoUtils)
        {
            dbContext = _dbContext;
            dbConnection = _dbConnection;
            userInfoUtils = _userInfoUtils;
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
            if (transaction.EditMode)
            {
                // Delete transaction in db because deemed easier to delete and recreate instead of update
                DeleteTransaction(transaction.JournalHeader.TransactionID);
            }

            // Check that user is not at transaction cap
            if (userInfoUtils.UserAtMaxTransactions(transaction.UserID))
            {
                return -1;
            }

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

        public JournalViewModel RetrieveAllTransactionsByUser(int userID)
        {
            string retrieveHeadersForUser = $"SELECT * FROM [Recording].JournalTransactions WHERE UserID = {userID} ORDER BY RecordedDateTime DESC";
            var headers = dbConnection.Query<JournalTransaction>(retrieveHeadersForUser).ToList();

            decimal? grandTotal = 0;
            List<JournalViewModel.Journal> transactions = new List<JournalViewModel.Journal>();
            foreach (JournalTransaction header in headers)
            {
                // Retrieve line items associated with current transaction
                string retrieveLineItems = $"SELECT * FROM [Recording].JournalEntries WHERE ParentTransactionID = {header.TransactionId}";
                var lineItems = dbConnection.Query<JournalEntry>(retrieveLineItems).ToList();

                JournalViewModel.Journal newEntry = new JournalViewModel.Journal()
                {
                    JournalHeader = header,
                    LineItems = lineItems
                };
                transactions.Add(newEntry);

                // Add current transaction lineItems to grandTotal
                foreach (var item in lineItems)
                {
                    grandTotal += (item.DebitAmount != null) ? item.DebitAmount : 0;
                }
            }
            JournalViewModel journal = new JournalViewModel()
            {
                Transactions = transactions,
                GrandTotal = (decimal)grandTotal
            };
            return journal;
        }

        public TransactionViewModel ConvertToTransactionViewModel(int transactionID)
        {
            string retrieveHeader = $"SELECT * FROM [Recording].JournalTransactions WHERE TransactionID = {transactionID}";
            JournalTransaction header = dbConnection.QueryFirstOrDefault<JournalTransaction>(retrieveHeader);
            if (header == null)
            {
                return null;
            }

            string retrieveLineItems = $"SELECT * FROM [Recording].JournalEntries WHERE ParentTransactionID = {header.TransactionId}";
            var lineItems = dbConnection.Query<JournalEntry>(retrieveLineItems).ToList();
            if (lineItems == null)
            {
                return null;
            }

            TransactionViewModel viewModel = new TransactionViewModel();
            viewModel.JournalHeader.Memo = header.Memo;
            viewModel.JournalHeader.TransactionID = header.TransactionId;
            viewModel.DefaultEntryDate = DateTime.Parse(header.RecordedDateTime);
            viewModel.UserID = header.UserID;
            viewModel.EditMode = true;

            foreach (JournalEntry item in lineItems)
            {
                JournalLineItemViewModel newItem = new JournalLineItemViewModel();
                newItem.AccountName = item.AccountName;
                if (item.DebitAmount != null)
                {
                    newItem.DebitAmount = (decimal)item.DebitAmount;
                }
                else
                {
                    newItem.CreditAmount = (decimal)item.CreditAmount;
                }
                viewModel.PreviousEntries.Add(newItem);
            }
            return viewModel;
        }

        public void DeleteTransaction(int transactionID)
        {
            string deleteTransaction = $"DELETE FROM [Recording].[JournalTransactions] WHERE TransactionID = {transactionID}";
            dbConnection.Execute(deleteTransaction);
            dbConnection.Close();
        }

        public Dictionary<string, LedgerAccount> Post(int userID)
        {
            Dictionary<string, LedgerAccount> accounts = new Dictionary<string, LedgerAccount>();

            string retrieveHeadersForUser = $"SELECT * FROM [Recording].[JournalTransactions] WHERE UserID = {userID}";
            var headers = dbConnection.Query<JournalTransaction>(retrieveHeadersForUser).ToList();
            if (headers.Count == 0)
            {
                return accounts;
            }

            // Build set of transactionIDs
            StringBuilder setOfIDs = new StringBuilder();
            for (int i = 0; i < headers.Count; i++)
            {
                if (i == headers.Count-1)
                {
                    setOfIDs.Append(headers[i].TransactionId);
                }
                else
                {
                    setOfIDs.Append(headers[i].TransactionId);
                    setOfIDs.Append(",");
                }
            }

            string retrieveAllLineItems = $"SELECT * FROM [Recording].[JournalEntries] WHERE ParentTransactionID IN ({setOfIDs})";
            var lineItems = dbConnection.Query<JournalEntry>(retrieveAllLineItems).ToList();

            foreach (JournalEntry item in lineItems)
            {
                if (accounts.ContainsKey(item.AccountName))
                {
                    if (accounts[item.AccountName].IsDebit)
                    {
                        accounts[item.AccountName].Amount += (item.DebitAmount != null) ? (decimal)item.DebitAmount : ((decimal)item.CreditAmount * -1);
                    }
                    else
                    {
                        accounts[item.AccountName].Amount += (item.DebitAmount != null) ? ((decimal)item.DebitAmount * -1) : (decimal)item.CreditAmount;
                    }
                }
                else
                {
                    LedgerAccount newAccount = new LedgerAccount();
                    newAccount.IsDebit = item.DebitBalance;
                    newAccount.AccountType = AccountsInfo.AccountTypes[item.AccountName];
                    if (item.DebitBalance)
                    {
                        newAccount.Amount = (item.DebitAmount != null) ? (decimal)item.DebitAmount : ((decimal)item.CreditAmount * -1); 
                    }
                    else
                    {
                        newAccount.Amount = (item.DebitAmount != null) ? ((decimal)item.DebitAmount * -1) : (decimal)item.CreditAmount;
                    }
                    accounts.Add(item.AccountName, newAccount);
                }
            }
            return accounts;
        }
    }
}
