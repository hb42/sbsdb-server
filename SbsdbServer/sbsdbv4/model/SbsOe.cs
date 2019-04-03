using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_OE")]
    public partial class SbsOe
    {
        public SbsOe()
        {
            InverseParentOeNavigation = new HashSet<SbsOe>();
            SbsApOeIndexNavigation = new HashSet<SbsAp>();
            SbsApStandortIndexNavigation = new HashSet<SbsAp>();
            SbsSw = new HashSet<SbsSw>();
        }

        [Column("OE_INDEX", TypeName = "bigint(20)")]
        public long OeIndex { get; set; }
        [Column("AP", TypeName = "bigint(20)")]
        public long? Ap { get; set; }
        [Required]
        [Column("BETRIEBSSTELLE", TypeName = "varchar(50)")]
        public string Betriebsstelle { get; set; }
        [Column("BST", TypeName = "bigint(20)")]
        public long Bst { get; set; }
        [Column("FAX", TypeName = "varchar(50)")]
        public string Fax { get; set; }
        [Column("OEFF", TypeName = "longtext")]
        public string Oeff { get; set; }
        [Column("TEL", TypeName = "varchar(50)")]
        public string Tel { get; set; }
        [Column("PARENT_OE", TypeName = "bigint(20)")]
        public long ParentOe { get; set; }
        [Column("FILIALE_INDEX", TypeName = "bigint(20)")]
        public long? FilialeIndex { get; set; }

        [ForeignKey("FilialeIndex")]
        [InverseProperty("SbsOe")]
        public virtual SbsFiliale FilialeIndexNavigation { get; set; }
        [ForeignKey("ParentOe")]
        [InverseProperty("InverseParentOeNavigation")]
        public virtual SbsOe ParentOeNavigation { get; set; }
        [InverseProperty("ParentOeNavigation")]
        public virtual ICollection<SbsOe> InverseParentOeNavigation { get; set; }
        [InverseProperty("OeIndexNavigation")]
        public virtual ICollection<SbsAp> SbsApOeIndexNavigation { get; set; }
        [InverseProperty("StandortIndexNavigation")]
        public virtual ICollection<SbsAp> SbsApStandortIndexNavigation { get; set; }
        [InverseProperty("OeIndexNavigation")]
        public virtual ICollection<SbsSw> SbsSw { get; set; }
    }
}
