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
        public int UserID { get; set; }
        public bool EditMode { get; set; }

        public TransactionViewModel()
        {
            JournalHeader = new JournalHeaderViewModel();
            JournalLineItem = new JournalLineItemViewModel();
            PreviousEntries = new List<JournalLineItemViewModel>();
            Action = null;
            EditMode = false;
        }
    }

    public class JournalHeaderViewModel
    {
        [StringLength(350, ErrorMessage = "Memo cannot exceed {1} characters.")]
        public string Memo { get; set; }
        public int TransactionID { get; set; }

    }

    public class JournalLineItemViewModel
    {
        [StringLength(60, ErrorMessage = "Account Name cannot exceed {1} characters.")]
        public string AccountName { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 9999999999.99)]
        public decimal? DebitAmount { get; set; }

        [DataType(DataType.Currency)]
        [Range(0, 9999999999.99)]
        public decimal? CreditAmount { get; set; }

        public int ActionItemIndex { get; set; }
    }
}
