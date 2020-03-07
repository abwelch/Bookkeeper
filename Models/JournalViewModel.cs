using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bookkeeper.Models
{
    // Wrapper for multiple inputs and redisplaying of prior inputs
    public class JournalViewModel
    {
        public List<JournalEntryViewModel> PreviousEntries;

        public class JournalTransactionViewModel
        {
            public string Memo { get; set; }
            public DateTime RecordedDate { get; set; }
            public TimeSpan RecordedTime { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            [Range(0, 99999999999.99)]
            public decimal TotalAmount { get; set; }
        }

        public class JournalEntryViewModel
        {
            [Required]
            public string AccountName { get; set; }

            public string AccountType { get; set; }

            public bool IsDebit { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            [Range(0, 99999999999.99)]
            public decimal Amount { get; set; }
        }
    }
}
