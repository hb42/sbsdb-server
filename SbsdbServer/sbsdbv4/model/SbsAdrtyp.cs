using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_ADRTYP")]
    public partial class SbsAdrtyp
    {
        public SbsAdrtyp()
        {
            SbsApAdr = new HashSet<SbsApAdr>();
        }

        [Column("ADR_INDEX", TypeName = "bigint(20)")]
        public long AdrIndex { get; set; }
        [Required]
        [Column("ADR_TYP", TypeName = "varchar(50)")]
        public string AdrTyp { get; set; }
        [Column("FLAG", TypeName = "bigint(20)")]
        public long? Flag { get; set; }
        [Column("PARAM", TypeName = "varchar(200)")]
        public string Param { get; set; }
        [Column("APTYP_INDEX", TypeName = "bigint(20)")]
        public long? AptypIndex { get; set; }

        [ForeignKey("AptypIndex")]
        [InverseProperty("SbsAdrtyp")]
        public virtual SbsAptyp AptypIndexNavigation { get; set; }
        [InverseProperty("AdrIndexNavigation")]
        public virtual ICollection<SbsApAdr> SbsApAdr { get; set; }
    }
}
