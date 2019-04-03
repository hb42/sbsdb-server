using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_SW_VV")]
    public partial class SbsSwVv
    {
        [Column("SWVV_INDEX", TypeName = "bigint(20)")]
        public long SwvvIndex { get; set; }
        [Column("DATUM", TypeName = "date")]
        public DateTime? Datum { get; set; }
        [Column("DOKLINK", TypeName = "longtext")]
        public string Doklink { get; set; }
        [Column("KUNDEN_DATEN", TypeName = "bit(1)")]
        public bool? KundenDaten { get; set; }
        [Column("MA_DATEN", TypeName = "bit(1)")]
        public bool? MaDaten { get; set; }
        [Column("VERSION", TypeName = "varchar(50)")]
        public string Version { get; set; }
        [Column("SW_INDEX", TypeName = "bigint(20)")]
        public long? SwIndex { get; set; }

        [ForeignKey("SwIndex")]
        [InverseProperty("SbsSwVv")]
        public virtual SbsSw SwIndexNavigation { get; set; }
    }
}
