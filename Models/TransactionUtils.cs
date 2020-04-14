using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookkeeper.Data;
using Bookkeeper.Models;

namespace Bookkeeper.Models
{

    public interface ITransactionUtils
    {
        bool ValidateTransaction(TransactionViewModel transaction);
        bool CommitTransaction(TransactionViewModel transaction);
    }

    public class TransactionUtils : ITransactionUtils
    {
        private readonly BookkeeperContext dbContext;

        public TransactionUtils(BookkeeperContext _dbContext)
        {
            dbContext = _dbContext;
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

        public bool CommitTransaction(TransactionViewModel transaction)
        {
            decimal? total = 0;
            foreach (JournalLineItemViewModel lineItem in transaction.PreviousEntries)
            {
                total += lineItem.DebitAmount;
            }
            JournalTransaction transactionEntry = new JournalTransaction()
            {
                Memo = transaction.JournalHeader.Memo,
                RecordedDate = transaction.JournalHeader.RecordedDate,
                RecordedTime = transaction.JournalHeader.RecordedDate.TimeOfDay,
                TotalAmount = (decimal)total,
                UserID = transaction.UserID 
            };
            dbContext.JournalTransactions.Add(transactionEntry);

            return true;
        }


    }
}
