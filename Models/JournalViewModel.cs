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

        public static class GenericAccountNames
        {
            #region [Balance Sheet]
            // Assets
            const string AccountsReceivable = "Accounts Receivable";
            const string AccumulatedDepreciation = "Accumulated Depreciation";
            const string AllowanceForDoubtfulAccounts = "Allowance for Doubtful Accounts";
            const string Buildings = "Buildings";
            const string Cash = "Cash";
            const string Equipment = "Equipment";
            const string Inventory = "Inventory";
            const string Land = "Land";
            const string Supplies = "Supplies";
            // Liabilities
            const string AccountsPayable = "Accounts Payable";
            const string AccruedExpenses = "Accrued Expenses";
            const string InstallmentLoansPayable = "Installment Loans Payable";
            const string ShortTermLoansPayable = "Short-term Loans Payable";
            const string UnearnedRevenue = "Unearned Revenue";
            // Stockholder's Equity
            const string CommonStock = "Common Stock";
            const string PaidInCapitalInExcessCS = "Paid-in Capital in Excess of Par Value - Common Stock";
            const string PaidInCapitalInExcessPS = "Paid-in Capital in Excess of Par Value - Preferred Stock";
            const string PreferredStock = "Preferred Stock";
            const string RetainedEarnings = "Retained Earnings";
            #endregion [Balance Sheet]

            #region [Income Statement]
            // Operating Revenues
            const string FeesEarned = "Fees Earned";
            const string Sales = "Sales";
            const string Services = "Services";
            // Operating Expenses
            const string CostOfGoodsSold = "Cost of Goods Sold";
            const string Rent = "Rent";
            const string Repairs = "Repairs";
            const string Salaries = "Salaries";
            const string Utilities = "Utilities";
            #endregion [Income Statement]

        }
    }
}
