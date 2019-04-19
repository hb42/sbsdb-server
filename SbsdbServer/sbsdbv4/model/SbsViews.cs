using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model {
    [Table("SBS_VIEWS")]
    public class SbsViews {
        [Column("VIEW_INDEX", TypeName = "bigint(20)")]
        public long ViewIndex { get; set; }

        [Required]
        [Column("VIEW_NAME", TypeName = "varchar(50)")]
        public string ViewName { get; set; }

        [Required]
        [Column("VIEW_SQL", TypeName = "longtext")]
        public string ViewSql { get; set; }

        [Required]
        [Column("VIEW_TYPE", TypeName = "varchar(1)")]
        public string ViewType { get; set; }
    }
}
