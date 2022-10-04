namespace TablicaDIM.DBModels
{
    public partial class TblWorkInWeekend
    {
        public int IdworkInWeekend { get; set; }
        public int WeekNumber { get; set; }
        public string YearNumber { get; set; } = null!;
        public int PersonId { get; set; }
        public int Shift { get; set; }
        public int Day { get; set; }
        public string Reason { get; set; } = null!;

        public virtual TblPerson Person { get; set; } = null!;
    }
}
