using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model {
    [Table("SBS_LIZTYP")]
    public class SbsLiztyp {
        public SbsLiztyp() {
            SbsSw = new HashSet<SbsSw>();
        }

        [Column("LIZTYP_INDEX", TypeName = "bigint(20)")]
        public long LiztypIndex { get; set; }

        [Column("ABRECHNUNG", TypeName = "bigint(20)")]
        public long Abrechnung { get; set; }

        [Required]
        [Column("LIZENZIERUNG", TypeName = "varchar(50)")]
        public string Lizenzierung { get; set; }

        [InverseProperty("LiztypIndexNavigation")]
        public virtual ICollection<SbsSw> SbsSw { get; set; }
    }
}
