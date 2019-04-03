using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_SW_OPDV")]
    public partial class SbsSwOpdv
    {
        [Column("SWOPDV_INDEX", TypeName = "bigint(20)")]
        public long SwopdvIndex { get; set; }
        [Column("DATUM", TypeName = "date")]
        public DateTime? Datum { get; set; }
        [Column("DOKLINK", TypeName = "longtext")]
        public string Doklink { get; set; }
        [Column("FREIGABE", TypeName = "varchar(50)")]
        public string Freigabe { get; set; }
        [Required]
        [Column("RISIKOSTUFE", TypeName = "varchar(50)")]
        public string Risikostufe { get; set; }
        [Column("VERSION", TypeName = "varchar(50)")]
        public string Version { get; set; }
        [Column("SW_INDEX", TypeName = "bigint(20)")]
        public long? SwIndex { get; set; }

        [ForeignKey("SwIndex")]
        [InverseProperty("SbsSwOpdv")]
        public virtual SbsSw SwIndexNavigation { get; set; }
    }
}
