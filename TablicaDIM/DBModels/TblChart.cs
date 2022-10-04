using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblChart
    {
        public int ChartId { get; set; }
        public int ShopId { get; set; }
        public int NumberOfWeek { get; set; }
        public int Year { get; set; }
        public double PercentOfBreakdown { get; set; }
        public int CoutOfBreakdown { get; set; }
        public double Mttr { get; set; }
        public double Mtbf { get; set; }
        public string AddWho { get; set; } = null!;
        public DateTime AddWhen { get; set; }
        public string? ModWho { get; set; }
        public DateTime? ModWhen { get; set; }

        public virtual TblShop Shop { get; set; } = null!;
    }
}
