using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_USER")]
    public partial class SbsUser
    {
        public SbsUser()
        {
            SbsPrefs = new HashSet<SbsPrefs>();
            SbsTtIssue = new HashSet<SbsTtIssue>();
        }

        [Column("USER_INDEX", TypeName = "bigint(20)")]
        public long UserIndex { get; set; }
        [Column("NACHNAME", TypeName = "varchar(50)")]
        public string Nachname { get; set; }
        [Required]
        [Column("PASSWORD", TypeName = "varchar(50)")]
        public string Password { get; set; }
        [Column("ROLLE", TypeName = "bigint(20)")]
        public long Rolle { get; set; }
        [Required]
        [Column("USER_ID", TypeName = "varchar(50)")]
        public string UserId { get; set; }
        [Column("VORNAME", TypeName = "varchar(50)")]
        public string Vorname { get; set; }

        [InverseProperty("UserIndexNavigation")]
        public virtual ICollection<SbsPrefs> SbsPrefs { get; set; }
        [InverseProperty("UserIndexNavigation")]
        public virtual ICollection<SbsTtIssue> SbsTtIssue { get; set; }
    }
}
