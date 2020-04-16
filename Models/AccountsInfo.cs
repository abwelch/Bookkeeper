using System.Collections.Generic;

namespace Bookkeeper.Models
{
    public static class AccountsInfo
    {
        public static List<string> AccountNames { get; }
        public static List<string> DebitAccounts { get; }
        public static List<string> CreditAccounts { get; }


        static AccountsInfo()
        {
            AccountNames = new List<string>
            {
                // Assets
                "Accounts Receivable",
                "Accumulated Depreciation",
                "Allowance for Doubtful Accounts",
                "Buildings",
                "Cash",
                "Equipment",
                "Inventory",
                "Land",
                "Supplies",
                // Liabilities
                "Accrued Expenses",
                "Installment Loans Payable",
                "Short-term Loans Payable",
                "Unearned Revenue",
                "Accounts Payable",
                // Stockholder's Equity
                "Common Stock",
                "Paid-in Capital in Excess of Par Value - Common Stock",
                "Paid-in Capital in Excess of Par Value - Preferred Stock",
                "Preferred Stock",
                "Retained Earnings",
                // Operating Revenues
                "Fees Earned",
                "Sales",
                "Services",
                // Operating Expenses
                "Cost of Goods Sold",
                "Rent",
                "Repairs",
                "Salaries",
                "Utilities"
            };

            DebitAccounts = new List<string>
            {
                "Accounts Receivable",
                "Accumulated Depreciation",
                "Allowance for Doubtful Accounts",
                "Buildings",
                "Cash",
                "Equipment",
                "Inventory",
                "Land",
                "Supplies",
                "Cost of Goods Sold"
            };

            CreditAccounts = new List<string>
            {
                "Accrued Expenses",
                "Installment Loans Payable",
                "Short-term Loans Payable",
                "Unearned Revenue",
                "Accounts Payable",
                "Common Stock",
                "Paid-in Capital in Excess of Par Value - Common Stock",
                "Paid-in Capital in Excess of Par Value - Preferred Stock",
                "Preferred Stock",
                "Retained Earnings",
                "Fees Earned",
                "Sales",
                "Services",
                "Rent",
                "Repairs",
                "Salaries",
                "Utilities"
            };

            AccountNames.Sort();
        }
    }
}
