using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_APKLASSE")]
    public partial class SbsApklasse
    {
        public SbsApklasse()
        {
            SbsAp = new HashSet<SbsAp>();
            SbsExtprog = new HashSet<SbsExtprog>();
        }

        [Column("APKLASSE_INDEX", TypeName = "bigint(20)")]
        public long ApklasseIndex { get; set; }
        [Required]
        [Column("APKLASSE", TypeName = "varchar(50)")]
        public string Apklasse { get; set; }
        [Column("FLAG", TypeName = "bigint(20)")]
        public long? Flag { get; set; }
        [Column("APTYP_INDEX", TypeName = "bigint(20)")]
        public long? AptypIndex { get; set; }

        [ForeignKey("AptypIndex")]
        [InverseProperty("SbsApklasse")]
        public virtual SbsAptyp AptypIndexNavigation { get; set; }
        [InverseProperty("ApklasseIndexNavigation")]
        public virtual ICollection<SbsAp> SbsAp { get; set; }
        [InverseProperty("ApklasseIndexNavigation")]
        public virtual ICollection<SbsExtprog> SbsExtprog { get; set; }
    }
}
