using System;
using System.Collections.Generic;

namespace Bookkeeper.Models
{
    // Should have been better named but tables already created and too lazy to redo
    // This class is a "Transaction Header" and is the parent to a number of child JournalEntry objects
    public partial class JournalTransaction
    {
        public JournalTransaction()
        {
            JournalEntries = new HashSet<JournalEntry>();
        }

        public int TransactionId { get; set; }
        public string Memo { get; set; }
        public DateTime RecordedDate { get; set; }
        public TimeSpan RecordedTime { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }

        public virtual UserInfo User { get; set; }
        public virtual ICollection<JournalEntry> JournalEntries { get; set; }
    }
}
