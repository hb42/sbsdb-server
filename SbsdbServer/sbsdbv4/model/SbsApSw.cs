using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_AP_SW")]
    public partial class SbsApSw
    {
        [Column("APSW_INDEX", TypeName = "bigint(20)")]
        public long ApswIndex { get; set; }
        [Column("ANZAHL", TypeName = "bigint(20)")]
        public long Anzahl { get; set; }
        [Column("VER", TypeName = "varchar(50)")]
        public string Ver { get; set; }
        [Column("AP_INDEX", TypeName = "bigint(20)")]
        public long ApIndex { get; set; }
        [Column("SW_INDEX", TypeName = "bigint(20)")]
        public long SwIndex { get; set; }

        [ForeignKey("ApIndex")]
        [InverseProperty("SbsApSw")]
        public virtual SbsAp ApIndexNavigation { get; set; }
        [ForeignKey("SwIndex")]
        [InverseProperty("SbsApSw")]
        public virtual SbsSw SwIndexNavigation { get; set; }
    }
}
