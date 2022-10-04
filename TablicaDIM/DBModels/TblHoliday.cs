using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblHoliday
    {
        public int HolidayId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool ItsFreeDay { get; set; }
    }
}
