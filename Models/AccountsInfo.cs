using System.Collections.Generic;

namespace Bookkeeper.Models
{
    public static class AccountsInfo
    {
        public static List<string> AccountNames { get; }
        public static List<string> DebitAccounts { get; }
        public static Dictionary<string, string> AccountTypes { get; }

        static AccountsInfo()
        {
            AccountTypes = new Dictionary<string, string>
            {
                { "Accounts Receivable", "Asset" },
                { "Buildings", "Asset" },
                { "Cash", "Asset" },
                { "Equipment", "Asset" },
                { "Inventory", "Asset" },
                { "Supplies", "Asset" },
                { "Accumulated Depreciation", "Contra Asset" },
                { "Allowance for Doubtful Accounts", "Contra Asset" },
                { "Accrued Expenses", "Liability" },
                { "Installment Loans Payable", "Liability" },
                { "Short-term Loans Payable", "Liability" },
                { "Unearned Revenue", "Liability" },
                { "Accounts Payable", "Liability" },
                { "Common Stock", "Owner's Equity" },
                { "Paid-in Capital in Excess of Par Value - Common Stock", "Owner's Equity" },
                { "Paid-in Capital in Excess of Par Value - Preferred Stock", "Owner's Equity" },
                { "Preferred Stock", "Owner's Equity" },
                { "Retained Earnings", "Owner's Equity" },
                { "Fees Earned", "Operating Revenue" },
                { "Sales", "Operating Revenue" },
                { "Services", "Operating Revenue" },
                { "Cost of Goods Sold", "Operating Expense" },
                { "Rent", "Operating Expense" },
                { "Repairs", "Operating Expense" },
                { "Salaries", "Operating Expense" },
                { "Utilities", "Operating Expense" },
            };

            AccountNames = new List<string>
            {
                // Assets
                "Accounts Receivable",
                "Buildings",
                "Cash",
                "Equipment",
                "Inventory",
                "Land",
                "Supplies",
                // Contra Assets
                "Accumulated Depreciation",
                "Allowance for Doubtful Accounts",
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
                "Cost of Goods Sold",
                "Rent",
                "Repairs",
                "Salaries",
                "Utilities"
            };

            AccountNames.Sort();
        }
    }
}
