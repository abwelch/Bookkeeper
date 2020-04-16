using System.Collections.Generic;

namespace Bookkeeper.Models
{
    public class BalanceSheetViewModel
    {
        public Dictionary<string, decimal> CurrentAssets { get; set; }
        public decimal TotalCurrentAssets { get; set; }
        public Dictionary<string, decimal> PropertyPlantEquipment { get; set; }
        public decimal TotalPPE { get; set; }
        public Dictionary<string, decimal> CurrentLiabilities { get; set; }
        public decimal TotalCurrentLiabilities { get; set; }
        public Dictionary<string, decimal> LongtermLiabilities { get; set; }
        public decimal TotalLongtermLiabilities { get; set; }
        public Dictionary<string, decimal> StockHolderEquity { get; set; }
        public decimal TotalStockHolderEquity { get; set; }

        public BalanceSheetViewModel()
        {
            CurrentAssets = new Dictionary<string, decimal>();
            PropertyPlantEquipment = new Dictionary<string, decimal>();
            CurrentLiabilities = new Dictionary<string, decimal>();
            LongtermLiabilities = new Dictionary<string, decimal>();
            StockHolderEquity = new Dictionary<string, decimal>();
        }
    }
}
