using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblPlace
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; } = null!;
        public int ShopId { get; set; }
        public string AddWho { get; set; } = null!;
        public DateTime AddWhen { get; set; }
        public string? ModWho { get; set; }
        public DateTime? ModWhen { get; set; }

        public virtual TblShop Shop { get; set; } = null!;
    }
}
