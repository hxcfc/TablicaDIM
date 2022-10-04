using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblUserSelectedShop
    {
        public int DataId { get; set; }
        public string UserPc { get; set; } = null!;
        public int? ShopId { get; set; }

        public virtual TblShop? Shop { get; set; }
    }
}
