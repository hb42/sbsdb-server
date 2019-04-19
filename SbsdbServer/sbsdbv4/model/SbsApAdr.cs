using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model {
    [Table("SBS_AP_ADR")]
    public class SbsApAdr {
        [Column("APADR_INDEX", TypeName = "bigint(20)")]
        public long ApadrIndex { get; set; }

        [Required]
        [Column("ADR_TEXT", TypeName = "varchar(50)")]
        public string AdrText { get; set; }

        [Column("ADR_INDEX", TypeName = "bigint(20)")]
        public long AdrIndex { get; set; }

        [Column("AP_INDEX", TypeName = "bigint(20)")]
        public long ApIndex { get; set; }

        [ForeignKey("AdrIndex")]
        [InverseProperty("SbsApAdr")]
        public virtual SbsAdrtyp AdrIndexNavigation { get; set; }

        [ForeignKey("ApIndex")]
        [InverseProperty("SbsApAdr")]
        public virtual SbsAp ApIndexNavigation { get; set; }
    }
}
