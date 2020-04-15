using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeper.Models
{
    public class TransactionSummaryViewModel
    {
        public JournalTransaction JournalHeader { get; set; }

        public List<JournalEntry> LineItems { get; set; }

    }
}
