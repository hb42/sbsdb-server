using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_SEGMENT")]
    public partial class SbsSegment
    {
        public SbsSegment()
        {
            SbsAp = new HashSet<SbsAp>();
        }

        [Column("SEGMENT_INDEX", TypeName = "bigint(20)")]
        public long SegmentIndex { get; set; }
        [Column("NETMASK", TypeName = "bigint(20)")]
        public long Netmask { get; set; }
        [Required]
        [Column("SEGMENT_NAME", TypeName = "varchar(50)")]
        public string SegmentName { get; set; }
        [Column("TCP", TypeName = "bigint(20)")]
        public long Tcp { get; set; }
        [Column("FILIALE_INDEX", TypeName = "bigint(20)")]
        public long? FilialeIndex { get; set; }

        [ForeignKey("FilialeIndex")]
        [InverseProperty("SbsSegment")]
        public virtual SbsFiliale FilialeIndexNavigation { get; set; }
        [InverseProperty("SegmentIndexNavigation")]
        public virtual ICollection<SbsAp> SbsAp { get; set; }
    }
}
