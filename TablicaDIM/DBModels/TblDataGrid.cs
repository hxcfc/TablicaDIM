using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblDataGrid
    {
        public int DataId { get; set; }
        public int PlaceId { get; set; }
        public int ShopId { get; set; }
        public int PersonId { get; set; }
        public DateTime DateCreateAction { get; set; }
        public DateTime DatePlaningAction { get; set; }
        public string Problem { get; set; } = null!;
        public string Cause { get; set; } = null!;
        public string Solve { get; set; } = null!;
        public DateTime? DateEndAction { get; set; }
        public int Step { get; set; }
        public string? Info { get; set; }
        public bool EndAction { get; set; }
        public string AddWho { get; set; } = null!;
        public DateTime AddWhen { get; set; }
        public string? ModWho { get; set; }
        public DateTime? ModWhen { get; set; }

        public virtual TblShop Shop { get; set; } = null!;
    }
}
