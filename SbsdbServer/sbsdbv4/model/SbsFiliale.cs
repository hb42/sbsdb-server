using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_FILIALE")]
    public partial class SbsFiliale
    {
        public SbsFiliale()
        {
            SbsOe = new HashSet<SbsOe>();
            //SbsSegment = new HashSet<SbsSegment>();
        }

        [Column("FILIALE_INDEX", TypeName = "bigint(20)")]
        public long FilialeIndex { get; set; }
        [Column("HAUSNR", TypeName = "varchar(50)")]
        public string Hausnr { get; set; }
        [Column("ORT", TypeName = "varchar(50)")]
        public string Ort { get; set; }
        [Column("PLZ", TypeName = "varchar(50)")]
        public string Plz { get; set; }
        [Column("STRASSE", TypeName = "varchar(50)")]
        public string Strasse { get; set; }

        [InverseProperty("FilialeIndexNavigation")]
        public virtual ICollection<SbsOe> SbsOe { get; set; }
        //[InverseProperty("FilialeIndexNavigation")]
        //public virtual ICollection<SbsSegment> SbsSegment { get; set; }
    }
}
