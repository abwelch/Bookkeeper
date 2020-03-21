using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bookkeeper.Models
{
    // Class for inputs and redisplaying of prior inputs
    public class TransactionViewModel
    {
        public JournalHeaderViewModel JournalHeader { get; set; }
        public JournalLineItemViewModel JournalLineItem { get; set; }
        public List<JournalLineItemViewModel> PreviousEntries { get; set; }
        public DateTime DefaultEntryDate { get; set; }
        public string Action { get; set; }
        public int ActionItemIndex { get; set; }
        public bool IsTransactionReady { get; set; }

        public TransactionViewModel()
        {
            JournalHeader = new JournalHeaderViewModel();
            JournalLineItem = new JournalLineItemViewModel();
            PreviousEntries = new List<JournalLineItemViewModel>();
            Action = null;
            IsTransactionReady = false;
        }
    }

    public class JournalHeaderViewModel
    {
        [StringLength(350, ErrorMessage = "Memo cannot exceed {1} characters.")]
        public string Memo { get; set; }

        [DataType(DataType.Date)]
        public DateTime RecordedDate { get; set; }
    }

    public class JournalLineItemViewModel
    {
        [StringLength(60, ErrorMessage = "Account Name cannot exceed {1} characters.")]
        public string AccountName { get; set; }

        public string AccountType { get; set; }

        public bool IsDebit { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 99999999999.99)]
        public decimal Amount { get; set; }
    }

    public static class JournalAction
    {
        public const string AddHeader = "AddHeader";
        public const string EditHeader = "EditHeader";
        public const string AddItem = "AddItem";
        public const string EditItem = "EditItem";
        public const string DeleteItem = "DeleteItem";
        public const string CommitTransaction = "CommitTransaction";
    }
}
