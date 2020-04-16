using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeper.Models
{
    public interface IStatementUtils
    {
        BalanceSheetViewModel GenerateBalanceSheet(int userID);
        decimal DetermineItemValue(JournalEntry item);
    }

    public class StatementUtils : IStatementUtils
    {
        private readonly IDbConnection dbConnection;

        public StatementUtils(IDbConnection _dbConnection)
        {
            dbConnection = _dbConnection;
        }

        public BalanceSheetViewModel GenerateBalanceSheet(int userID)
        {
            string retrieveHeadersForUser = $"SELECT * FROM [Recording].[JournalTransactions] WHERE UserID = {userID}";
            var headers = dbConnection.Query<JournalTransaction>(retrieveHeadersForUser).ToList(); 
            if (headers.Count == 0)
            {
                return null;
            }

            BalanceSheetViewModel model = new BalanceSheetViewModel();

            // Build set of transactionIDs
            StringBuilder setOfIDs = new StringBuilder();
            for (int i = 0; i < headers.Count; i++)
            {
                if (i == headers.Count - 1)
                {
                    setOfIDs.Append(headers[i].TransactionId);
                }
                else
                {
                    setOfIDs.Append(headers[i].TransactionId);
                    setOfIDs.Append(",");
                }
            }
            string retrieveAllLineItems = $"SELECT * FROM [Recording].[JournalEntries] WHERE ParentTransactionID IN ({setOfIDs})";
            var lineItems = dbConnection.Query<JournalEntry>(retrieveAllLineItems).ToList();

            foreach (JournalEntry item in lineItems)
            {
                // Disregard entries with accounts unrelated to balance sheet
                if (!AccountsInfo.BalanceSheetAccountTypes.ContainsKey(item.AccountName))
                {
                    continue;
                }

                if (model.CurrentAssets.ContainsKey(item.AccountName))
                {
                    model.CurrentAssets[item.AccountName] += DetermineItemValue(item);
                }
                else if (model.PropertyPlantEquipment.ContainsKey(item.AccountName))
                {
                    model.PropertyPlantEquipment[item.AccountName] += DetermineItemValue(item);
                }
                else if (model.CurrentLiabilities.ContainsKey(item.AccountName))
                {
                    model.CurrentLiabilities[item.AccountName] += DetermineItemValue(item);
                }
                else if (model.LongtermLiabilities.ContainsKey(item.AccountName))
                {
                    model.LongtermLiabilities[item.AccountName] += DetermineItemValue(item);
                }
                else if (model.StockHolderEquity.ContainsKey(item.AccountName))
                {
                    model.StockHolderEquity[item.AccountName] += DetermineItemValue(item);
                }
                else
                {
                    // Add new account to correct dictionary
                    string accountType = AccountsInfo.BalanceSheetAccountTypes[item.AccountName];
                    switch (accountType)
                    {
                        case BalanceSheetTypes.CurrentAsset:
                            model.CurrentAssets.Add(item.AccountName, DetermineItemValue(item));
                            break;
                        case BalanceSheetTypes.PPE:
                            model.PropertyPlantEquipment.Add(item.AccountName, DetermineItemValue(item));
                            break;
                        case BalanceSheetTypes.CurrentLiability:
                            model.CurrentLiabilities.Add(item.AccountName, DetermineItemValue(item));
                            break;
                        case BalanceSheetTypes.LongtermLiability:
                            model.LongtermLiabilities.Add(item.AccountName, DetermineItemValue(item));
                            break;
                        case BalanceSheetTypes.OwnerEquity:
                            model.StockHolderEquity.Add(item.AccountName, DetermineItemValue(item));
                            break;
                    }
                }
            }

            #region [ Calculate total amount for each group ]
            foreach (var account in model.CurrentAssets)
            {
                // Contra asset is handled differently
                if (account.Key == "Allowance for Doubtful Accounts")
                {
                    model.TotalCurrentAssets -= account.Value;
                }
                else
                {
                    model.TotalCurrentAssets += account.Value;
                }
            }

            foreach (var account in model.PropertyPlantEquipment)
            {
                // Contra asset is handled differently
                if (account.Key == "Accumulated Depreciation")
                {
                    model.TotalPPE -= account.Value;
                }
                else
                {
                    model.TotalPPE += account.Value;
                }
            }

            foreach (var account in model.CurrentLiabilities)
            {
                model.TotalCurrentLiabilities += account.Value;
            }

            foreach (var account in model.LongtermLiabilities)
            {
                model.TotalLongtermLiabilities += account.Value;
            }

            foreach (var account in model.StockHolderEquity)
            {
                model.TotalStockHolderEquity += account.Value;
            }
            #endregion [ Calculate total amount for each group ]

            return model;
        }

        public decimal DetermineItemValue(JournalEntry item) 
        {
            decimal? amount = 0;
            if (item.DebitBalance)
            {
                amount = (item.DebitAmount != null) ? (decimal)item.DebitAmount : ((decimal)item.CreditAmount * -1);
            }
            else
            {
                amount = (item.DebitAmount != null) ? ((decimal)item.DebitAmount * -1) : (decimal)item.CreditAmount;
            }
            return (decimal)amount;
        }
    }
}
