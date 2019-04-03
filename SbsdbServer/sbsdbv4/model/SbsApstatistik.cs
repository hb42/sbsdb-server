using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_APSTATISTIK")]
    public partial class SbsApstatistik
    {
        public SbsApstatistik()
        {
            SbsAp = new HashSet<SbsAp>();
        }

        [Column("APSTATISTIK_INDEX", TypeName = "bigint(20)")]
        public long ApstatistikIndex { get; set; }
        [Required]
        [Column("APSTATISTIK", TypeName = "varchar(50)")]
        public string Apstatistik { get; set; }
        [Column("FLAG", TypeName = "bigint(20)")]
        public long? Flag { get; set; }
        [Column("SORT", TypeName = "bigint(20)")]
        public long Sort { get; set; }
        [Column("APTYP_INDEX", TypeName = "bigint(20)")]
        public long? AptypIndex { get; set; }

        [ForeignKey("AptypIndex")]
        [InverseProperty("SbsApstatistik")]
        public virtual SbsAptyp AptypIndexNavigation { get; set; }
        [InverseProperty("ApstatistikIndexNavigation")]
        public virtual ICollection<SbsAp> SbsAp { get; set; }
    }
}
