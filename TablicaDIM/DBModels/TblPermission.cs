using System.Collections.Generic;

namespace TablicaDIM.DBModels
{
    public partial class TblPermission
    {
        public TblPermission()
        {
            TblPeople = new HashSet<TblPerson>();
        }

        public int PermissionId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<TblPerson> TblPeople { get; set; }
    }
}
