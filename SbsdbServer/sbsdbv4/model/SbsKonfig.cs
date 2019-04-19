using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model {
    [Table("SBS_KONFIG")]
    public class SbsKonfig {
        public SbsKonfig() {
            SbsHw = new HashSet<SbsHw>();
        }

        [Column("KONFIG_INDEX", TypeName = "bigint(20)")]
        public long KonfigIndex { get; set; }

        [Required]
        [Column("BEZEICHNUNG", TypeName = "varchar(50)")]
        public string Bezeichnung { get; set; }

        [Column("HD", TypeName = "varchar(50)")]
        public string Hd { get; set; }

        [Required]
        [Column("HERSTELLER", TypeName = "varchar(50)")]
        public string Hersteller { get; set; }

        [Column("PROZESSOR", TypeName = "varchar(50)")]
        public string Prozessor { get; set; }

        [Column("RAM", TypeName = "varchar(50)")]
        public string Ram { get; set; }

        [Column("SONST", TypeName = "longtext")]
        public string Sonst { get; set; }

        [Column("VIDEO", TypeName = "varchar(50)")]
        public string Video { get; set; }

        [Column("HWTYP_INDEX", TypeName = "bigint(20)")]
        public long? HwtypIndex { get; set; }

        [ForeignKey("HwtypIndex")]
        [InverseProperty("SbsKonfig")]
        public virtual SbsHwtyp HwtypIndexNavigation { get; set; }

        [InverseProperty("KonfigIndexNavigation")]
        public virtual ICollection<SbsHw> SbsHw { get; set; }
    }
}
