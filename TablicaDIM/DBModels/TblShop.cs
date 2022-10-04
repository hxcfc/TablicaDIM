using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblShop
    {
        public TblShop()
        {
            TblCharts = new HashSet<TblChart>();
            TblDataGrids = new HashSet<TblDataGrid>();
            TblPeople = new HashSet<TblPerson>();
            TblPersonInWeekends = new HashSet<TblPersonInWeekend>();
            TblPlaces = new HashSet<TblPlace>();
            TblShiftInWeekends = new HashSet<TblShiftInWeekend>();
            TblUserSelectedShops = new HashSet<TblUserSelectedShop>();
        }

        public int ShopId { get; set; }
        public string ShopName { get; set; } = null!;
        public DateTime AddWhen { get; set; }
        public string AddWho { get; set; } = null!;
        public string? ModWho { get; set; }
        public DateTime? ModWhen { get; set; }
        public int TechnicalWarning { get; set; }
        public int TechnicalAlarm { get; set; }
        public int OfficeWarning { get; set; }
        public int OfficeAlarm { get; set; }
        public double MinChartMttr { get; set; }
        public double MaxChartMttr { get; set; }
        public double ChartMttr { get; set; }
        public double MinChartMtbf { get; set; }
        public double MaxChartMtbf { get; set; }
        public double ChartMtbf { get; set; }
        public double MinChartPercentOfBreakdown { get; set; }
        public double MaxChartPercentOfBreakdown { get; set; }
        public double ChartPercentOfBreakdown { get; set; }
        public int MinChartCoutOfBreakdown { get; set; }
        public int MaxChartCoutOfBreakdown { get; set; }
        public int ChartCoutOfBreakdown { get; set; }
        public bool ShopInactive { get; set; }

        public virtual ICollection<TblChart> TblCharts { get; set; }
        public virtual ICollection<TblDataGrid> TblDataGrids { get; set; }
        public virtual ICollection<TblPerson> TblPeople { get; set; }
        public virtual ICollection<TblPersonInWeekend> TblPersonInWeekends { get; set; }
        public virtual ICollection<TblPlace> TblPlaces { get; set; }
        public virtual ICollection<TblShiftInWeekend> TblShiftInWeekends { get; set; }
        public virtual ICollection<TblUserSelectedShop> TblUserSelectedShops { get; set; }
    }
}
