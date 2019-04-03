using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_TT_KATEGORIE")]
    public partial class SbsTtKategorie
    {
        public SbsTtKategorie()
        {
            SbsTtIssue = new HashSet<SbsTtIssue>();
        }

        [Column("KATEGORIE_INDEX", TypeName = "bigint(20)")]
        public long KategorieIndex { get; set; }
        [Column("FLAG", TypeName = "bigint(20)")]
        public long? Flag { get; set; }
        [Required]
        [Column("KATEGORIE", TypeName = "varchar(50)")]
        public string Kategorie { get; set; }

        [InverseProperty("KategorieIndexNavigation")]
        public virtual ICollection<SbsTtIssue> SbsTtIssue { get; set; }
    }
}
