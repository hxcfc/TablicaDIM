using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblShiftInWeekend
    {
        public int IdshopInWeekend { get; set; }
        public int ShopId { get; set; }
        public int YearNumber { get; set; }
        public int WeekNumber { get; set; }
        public int DayNumber { get; set; }
        public int Shift { get; set; }
        public int Count { get; set; }

        public virtual TblShop Shop { get; set; } = null!;
    }
}
