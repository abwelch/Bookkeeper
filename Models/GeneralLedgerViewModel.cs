using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeper.Models
{
    public class GeneralLedgerViewModel
    {
        public Dictionary<string, LedgerAccount> AllAccounts { get; set; }
    }

    public class LedgerAccount
    {
        public bool IsDebit { get; set; }
        public decimal Amount { get; set; }
        public string AccountType { get; set; }
    }
}
