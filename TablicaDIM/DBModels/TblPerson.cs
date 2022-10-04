using System;
using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblPerson
    {
        public TblPerson()
        {
            TblPersonInWeekends = new HashSet<TblPersonInWeekend>();
            TblUnavailables = new HashSet<TblUnavailable>();
        }

        public int PersonId { get; set; }
        public string Login { get; set; } = null!;
        public bool? FirstLogin { get; set; }
        public int PermisionId { get; set; }
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int ShopId { get; set; }
        public string AddWho { get; set; } = null!;
        public DateTime AddWhen { get; set; }
        public string? ModWho { get; set; }
        public DateTime? ModWhen { get; set; }
        public int? Shift { get; set; }
        public int FreeDays { get; set; }

        public virtual TblShop Shop { get; set; } = null!;
        public virtual ICollection<TblPersonInWeekend> TblPersonInWeekends { get; set; }
        public virtual ICollection<TblUnavailable> TblUnavailables { get; set; }
    }
}
