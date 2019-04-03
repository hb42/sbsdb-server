using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_PREFS")]
    public partial class SbsPrefs
    {
        [Column("PREFS_INDEX", TypeName = "bigint(20)")]
        public long PrefsIndex { get; set; }
        [Required]
        [Column("PREFERENCE", TypeName = "varchar(50)")]
        public string Preference { get; set; }
        [Column("TEXT", TypeName = "varchar(200)")]
        public string Text { get; set; }
        [Column("USER_INDEX", TypeName = "bigint(20)")]
        public long? UserIndex { get; set; }

        [ForeignKey("UserIndex")]
        [InverseProperty("SbsPrefs")]
        public virtual SbsUser UserIndexNavigation { get; set; }
    }
}
