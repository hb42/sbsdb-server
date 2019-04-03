using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_SW_BESTAND")]
    public partial class SbsSwBestand
    {
        [Column("SWBESTAND_INDEX", TypeName = "bigint(20)")]
        public long SwbestandIndex { get; set; }
        [Column("ANSCH_DAT", TypeName = "date")]
        public DateTime? AnschDat { get; set; }
        [Column("ANSCH_WERT")]
        public double? AnschWert { get; set; }
        [Column("ANZAHL", TypeName = "bigint(20)")]
        public long Anzahl { get; set; }
        [Column("INV_NR", TypeName = "varchar(50)")]
        public string InvNr { get; set; }
        [Column("SW_INDEX", TypeName = "bigint(20)")]
        public long? SwIndex { get; set; }

        [ForeignKey("SwIndex")]
        [InverseProperty("SbsSwBestand")]
        public virtual SbsSw SwIndexNavigation { get; set; }
    }
}
