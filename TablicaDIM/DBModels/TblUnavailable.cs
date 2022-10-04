using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblUnavailable
    {
        public int UnavailableId { get; set; }
        public int PersonId { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime AbsentFrom { get; set; }
        public DateTime AbsentTo { get; set; }
        public int DaysCount { get; set; }
        public DateTime DataOfSend { get; set; }
        public bool Accepted { get; set; }
        public bool ToDelete { get; set; }

        public virtual TblPerson Person { get; set; } = null!;
    }
}
