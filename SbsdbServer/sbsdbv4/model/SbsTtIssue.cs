using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_TT_ISSUE")]
    public partial class SbsTtIssue
    {
        [Column("TTISSUE_INDEX", TypeName = "bigint(20)")]
        public long TtissueIndex { get; set; }
        [Column("CLOSE", TypeName = "datetime")]
        public DateTime? Close { get; set; }
        [Column("OPEN", TypeName = "datetime")]
        public DateTime Open { get; set; }
        [Column("PRIO", TypeName = "bigint(20)")]
        public long Prio { get; set; }
        [Required]
        [Column("TICKET", TypeName = "longtext")]
        public string Ticket { get; set; }
        [Column("AP_INDEX", TypeName = "bigint(20)")]
        public long? ApIndex { get; set; }
        [Column("KATEGORIE_INDEX", TypeName = "bigint(20)")]
        public long KategorieIndex { get; set; }
        [Column("USER_INDEX", TypeName = "bigint(20)")]
        public long UserIndex { get; set; }

        [ForeignKey("ApIndex")]
        [InverseProperty("SbsTtIssue")]
        public virtual SbsAp ApIndexNavigation { get; set; }
        [ForeignKey("KategorieIndex")]
        [InverseProperty("SbsTtIssue")]
        public virtual SbsTtKategorie KategorieIndexNavigation { get; set; }
        [ForeignKey("UserIndex")]
        [InverseProperty("SbsTtIssue")]
        public virtual SbsUser UserIndexNavigation { get; set; }
    }
}
