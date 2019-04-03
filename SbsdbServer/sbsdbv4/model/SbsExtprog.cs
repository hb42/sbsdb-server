using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_EXTPROG")]
    public partial class SbsExtprog
    {
        [Column("EXTPROG_INDEX", TypeName = "bigint(20)")]
        public long ExtprogIndex { get; set; }
        [Required]
        [Column("EXTPROG", TypeName = "varchar(255)")]
        public string Extprog { get; set; }
        [Required]
        [Column("EXTPROG_NAME", TypeName = "varchar(50)")]
        public string ExtprogName { get; set; }
        [Column("EXTPROG_PAR", TypeName = "varchar(255)")]
        public string ExtprogPar { get; set; }
        [Column("FLAG", TypeName = "bigint(20)")]
        public long? Flag { get; set; }
        [Column("APKLASSE_INDEX", TypeName = "bigint(20)")]
        public long? ApklasseIndex { get; set; }

        [ForeignKey("ApklasseIndex")]
        [InverseProperty("SbsExtprog")]
        public virtual SbsApklasse ApklasseIndexNavigation { get; set; }
    }
}
