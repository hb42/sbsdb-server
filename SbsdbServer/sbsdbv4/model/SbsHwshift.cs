using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model {
    [Table("SBS_HWSHIFT")]
    public class SbsHwshift {
        [Column("HWSHIFT_INDEX", TypeName = "bigint(20)")]
        public long HwshiftIndex { get; set; }

        [Column("AP_INDEX", TypeName = "bigint(20)")]
        public long ApIndex { get; set; }

        [Column("BETRIEBSSTELLE", TypeName = "varchar(50)")]
        public string Betriebsstelle { get; set; }

        [Column("BEZEICHNUNG", TypeName = "varchar(55)")]
        public string Bezeichnung { get; set; }

        [Required]
        [Column("DIRECTION", TypeName = "varchar(2)")]
        public string Direction { get; set; }

        [Column("HOST", TypeName = "varchar(50)")]
        public string Host { get; set; }

        [Column("SHIFTDATE", TypeName = "timestamp")]
        public DateTime Shiftdate { get; set; }

        [Column("HW_INDEX", TypeName = "bigint(20)")]
        public long? HwIndex { get; set; }

        [ForeignKey("HwIndex")]
        [InverseProperty("SbsHwshift")]
        public virtual SbsHw HwIndexNavigation { get; set; }
    }
}
