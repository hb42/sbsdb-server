using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_SW")]
    public partial class SbsSw
    {
        public SbsSw()
        {
            SbsApSw = new HashSet<SbsApSw>();
            SbsSwBestand = new HashSet<SbsSwBestand>();
            SbsSwOpdv = new HashSet<SbsSwOpdv>();
            SbsSwVv = new HashSet<SbsSwVv>();
        }

        [Column("SW_INDEX", TypeName = "bigint(20)")]
        public long SwIndex { get; set; }
        [Column("BESCHREIBUNG", TypeName = "longtext")]
        public string Beschreibung { get; set; }
        [Required]
        [Column("BEZEICHNUNG", TypeName = "varchar(50)")]
        public string Bezeichnung { get; set; }
        [Column("EINSATZ", TypeName = "bit(1)")]
        public bool? Einsatz { get; set; }
        [Required]
        [Column("HERSTELLER", TypeName = "varchar(50)")]
        public string Hersteller { get; set; }
        [Column("INST_ORT", TypeName = "varchar(50)")]
        public string InstOrt { get; set; }
        [Column("LIZENZ_PA")]
        public double? LizenzPa { get; set; }
        [Column("NUTZER", TypeName = "varchar(100)")]
        public string Nutzer { get; set; }
        [Column("SB_INTEGR", TypeName = "varchar(20)")]
        public string SbIntegr { get; set; }
        [Column("SB_NACHV", TypeName = "varchar(20)")]
        public string SbNachv { get; set; }
        [Column("SB_RECHTL", TypeName = "varchar(50)")]
        public string SbRechtl { get; set; }
        [Column("SB_VERFUEG", TypeName = "varchar(20)")]
        public string SbVerfueg { get; set; }
        [Column("SB_VERTR", TypeName = "varchar(20)")]
        public string SbVertr { get; set; }
        [Column("SMS_PAKET", TypeName = "varchar(50)")]
        public string SmsPaket { get; set; }
        [Column("VOLLZ_ERKL", TypeName = "bit(1)")]
        public bool? VollzErkl { get; set; }
        [Column("LIZTYP_INDEX", TypeName = "bigint(20)")]
        public long? LiztypIndex { get; set; }
        [Column("OE_INDEX", TypeName = "bigint(20)")]
        public long? OeIndex { get; set; }

        [ForeignKey("LiztypIndex")]
        [InverseProperty("SbsSw")]
        public virtual SbsLiztyp LiztypIndexNavigation { get; set; }
        [ForeignKey("OeIndex")]
        [InverseProperty("SbsSw")]
        public virtual SbsOe OeIndexNavigation { get; set; }
        [InverseProperty("SwIndexNavigation")]
        public virtual ICollection<SbsApSw> SbsApSw { get; set; }
        [InverseProperty("SwIndexNavigation")]
        public virtual ICollection<SbsSwBestand> SbsSwBestand { get; set; }
        [InverseProperty("SwIndexNavigation")]
        public virtual ICollection<SbsSwOpdv> SbsSwOpdv { get; set; }
        [InverseProperty("SwIndexNavigation")]
        public virtual ICollection<SbsSwVv> SbsSwVv { get; set; }
    }
}
