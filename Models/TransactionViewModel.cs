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

        public int Action { get; set; }

        public int ActionItemIndex { get; set; } // Index of the list item to process the action on

        public TransactionViewModel()
        {
            JournalHeader = new JournalHeaderViewModel();
            JournalLineItem = new JournalLineItemViewModel();
            PreviousEntries = new List<JournalLineItemViewModel>();
            Action = -1;
            ActionItemIndex = -1;
        }
    }

    public class JournalHeaderViewModel
    {
        [Required]
        [StringLength(350, ErrorMessage = "Memo cannot exceed {1} characters.")]
        public string Memo { get; set; }

        [Required]
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

    public enum JournalAction
    {
        AddHeader,
        EditHeader,
        AddItem,
        EditItem,
        DeleteItem,
        CommitTransaction
    }
}
