using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblPersonInWeekend
    {
        public int IdworkInWeekend { get; set; }
        public int WeekNumber { get; set; }
        public int YearNumber { get; set; }
        public int PersonId { get; set; }
        public int ShiftDayIndex { get; set; }
        public string Reason { get; set; } = null!;
        public int ShopId { get; set; }

        public virtual TblPerson Person { get; set; } = null!;
        public virtual TblShop Shop { get; set; } = null!;
    }
}
