using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_HWTYP")]
    public partial class SbsHwtyp
    {
        public SbsHwtyp()
        {
            SbsKonfig = new HashSet<SbsKonfig>();
        }

        [Column("HWTYP_INDEX", TypeName = "bigint(20)")]
        public long HwtypIndex { get; set; }
        [Required]
        [Column("HWTYP", TypeName = "varchar(50)")]
        public string Hwtyp { get; set; }
        [Column("APTYP_INDEX", TypeName = "bigint(20)")]
        public long? AptypIndex { get; set; }

        [ForeignKey("AptypIndex")]
        [InverseProperty("SbsHwtyp")]
        public virtual SbsAptyp AptypIndexNavigation { get; set; }
        [InverseProperty("HwtypIndexNavigation")]
        public virtual ICollection<SbsKonfig> SbsKonfig { get; set; }
    }
}
