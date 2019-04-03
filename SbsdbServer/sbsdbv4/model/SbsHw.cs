using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_HW")]
    public partial class SbsHw
    {
        public SbsHw()
        {
            SbsHwshift = new HashSet<SbsHwshift>();
        }

        [Column("HW_INDEX", TypeName = "bigint(20)")]
        public long HwIndex { get; set; }
        [Column("ANSCH_DAT", TypeName = "date")]
        public DateTime? AnschDat { get; set; }
        [Column("ANSCH_WERT")]
        public double? AnschWert { get; set; }
        [Column("INV_NR", TypeName = "varchar(50)")]
        public string InvNr { get; set; }
        [Column("MAC", TypeName = "varchar(50)")]
        public string Mac { get; set; }
        [Column("NETBOOTGUID", TypeName = "varchar(50)")]
        public string Netbootguid { get; set; }
        [Column("PRI", TypeName = "varchar(1)")]
        public string Pri { get; set; }
        [Required]
        [Column("SER_NR", TypeName = "varchar(50)")]
        public string SerNr { get; set; }
        [Column("WARTUNG_BEM", TypeName = "varchar(200)")]
        public string WartungBem { get; set; }
        [Column("WARTUNG_FA", TypeName = "varchar(50)")]
        public string WartungFa { get; set; }
        [Column("AP_INDEX", TypeName = "bigint(20)")]
        public long? ApIndex { get; set; }
        [Column("KONFIG_INDEX", TypeName = "bigint(20)")]
        public long? KonfigIndex { get; set; }

        [ForeignKey("ApIndex")]
        [InverseProperty("SbsHw")]
        public virtual SbsAp ApIndexNavigation { get; set; }
        [ForeignKey("KonfigIndex")]
        [InverseProperty("SbsHw")]
        public virtual SbsKonfig KonfigIndexNavigation { get; set; }
        [InverseProperty("HwIndexNavigation")]
        public virtual ICollection<SbsHwshift> SbsHwshift { get; set; }
    }
}
