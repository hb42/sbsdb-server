using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_AP")]
    public partial class SbsAp
    {
        public SbsAp()
        {
            SbsApAdr = new HashSet<SbsApAdr>();
            SbsApSw = new HashSet<SbsApSw>();
            SbsHw = new HashSet<SbsHw>();
            SbsTtIssue = new HashSet<SbsTtIssue>();
        }

        [Column("AP_INDEX", TypeName = "bigint(20)")]
        public long ApIndex { get; set; }
        [Required]
        [Column("AP_NAME", TypeName = "varchar(50)")]
        public string ApName { get; set; }
        [Column("BEMERKUNG", TypeName = "longtext")]
        public string Bemerkung { get; set; }
        [Required]
        [Column("BEZEICHNUNG", TypeName = "varchar(55)")]
        public string Bezeichnung { get; set; }
        [Column("TCP", TypeName = "bigint(20)")]
        public long Tcp { get; set; }
        [Column("APKLASSE_INDEX", TypeName = "bigint(20)")]
        public long? ApklasseIndex { get; set; }
        [Column("APSTATISTIK_INDEX", TypeName = "bigint(20)")]
        public long? ApstatistikIndex { get; set; }
        [Column("OE_INDEX", TypeName = "bigint(20)")]
        public long? OeIndex { get; set; }
        [Column("STANDORT_INDEX", TypeName = "bigint(20)")]
        public long? StandortIndex { get; set; }
        [Column("SEGMENT_INDEX", TypeName = "bigint(20)")]
        public long? SegmentIndex { get; set; }

        [ForeignKey("ApklasseIndex")]
        [InverseProperty("SbsAp")]
        public virtual SbsApklasse ApklasseIndexNavigation { get; set; }
        [ForeignKey("ApstatistikIndex")]
        [InverseProperty("SbsAp")]
        public virtual SbsApstatistik ApstatistikIndexNavigation { get; set; }
        [ForeignKey("OeIndex")]
        [InverseProperty("SbsApOeIndexNavigation")]
        public virtual SbsOe OeIndexNavigation { get; set; }
        [ForeignKey("SegmentIndex")]
        [InverseProperty("SbsAp")]
        public virtual SbsSegment SegmentIndexNavigation { get; set; }
        [ForeignKey("StandortIndex")]
        [InverseProperty("SbsApStandortIndexNavigation")]
        public virtual SbsOe StandortIndexNavigation { get; set; }
        [InverseProperty("ApIndexNavigation")]
        public virtual ICollection<SbsApAdr> SbsApAdr { get; set; }
        [InverseProperty("ApIndexNavigation")]
        public virtual ICollection<SbsApSw> SbsApSw { get; set; }
        [InverseProperty("ApIndexNavigation")]
        public virtual ICollection<SbsHw> SbsHw { get; set; }
        [InverseProperty("ApIndexNavigation")]
        public virtual ICollection<SbsTtIssue> SbsTtIssue { get; set; }
    }
}
