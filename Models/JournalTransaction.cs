using System;
using System.Collections.Generic;

namespace Bookkeeper.Models
{
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
