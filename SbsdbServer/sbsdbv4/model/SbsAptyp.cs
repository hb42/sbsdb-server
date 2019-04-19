using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model {
    [Table("SBS_APTYP")]
    public class SbsAptyp {
        public SbsAptyp() {
            SbsAdrtyp = new HashSet<SbsAdrtyp>();
            SbsApklasse = new HashSet<SbsApklasse>();
            SbsApstatistik = new HashSet<SbsApstatistik>();
            SbsHwtyp = new HashSet<SbsHwtyp>();
        }

        [Column("APTYP_INDEX", TypeName = "bigint(20)")]
        public long AptypIndex { get; set; }

        [Required]
        [Column("APTYP", TypeName = "varchar(50)")]
        public string Aptyp { get; set; }

        [Column("FLAG", TypeName = "bigint(20)")]
        public long? Flag { get; set; }

        [Column("LFD_NR", TypeName = "bigint(20)")]
        public long? LfdNr { get; set; }

        [Column("PARAM", TypeName = "varchar(200)")]
        public string Param { get; set; }

        [InverseProperty("AptypIndexNavigation")]
        public virtual ICollection<SbsAdrtyp> SbsAdrtyp { get; set; }

        [InverseProperty("AptypIndexNavigation")]
        public virtual ICollection<SbsApklasse> SbsApklasse { get; set; }

        [InverseProperty("AptypIndexNavigation")]
        public virtual ICollection<SbsApstatistik> SbsApstatistik { get; set; }

        [InverseProperty("AptypIndexNavigation")]
        public virtual ICollection<SbsHwtyp> SbsHwtyp { get; set; }
    }
}
