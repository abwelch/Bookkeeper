using System;
using System.Collections.Generic;

namespace Bookkeeper.Models
{
    public partial class JournalEntry
    {
        public int EntryId { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public bool IsDebit { get; set; }
        public decimal Amount { get; set; }
        public int ParentTransactionId { get; set; }

        public virtual JournalTransaction ParentTransaction { get; set; }
    }
}
