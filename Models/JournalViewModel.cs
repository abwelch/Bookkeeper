using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeper.Models
{
    public class JournalViewModel
    {
        public List<Journal> Transactions { get; set; }
        public decimal GrandTotal { get; set; }
        public List<int> TransactionIDs { get; set; }
        public string Action { get; set; }

        public class Journal
        {
            public JournalTransaction JournalHeader { get; set; }
            public List<JournalEntry> LineItems { get; set; }
        }
    }
    
}
