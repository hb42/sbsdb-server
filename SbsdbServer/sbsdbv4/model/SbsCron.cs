using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hb.SbsdbServer.sbsdbv4.model
{
    [Table("SBS_CRON")]
    public partial class SbsCron
    {
        [Column("CRON_INDEX", TypeName = "varchar(20)")]
        public string CronIndex { get; set; }
        [Column("CRON", TypeName = "varchar(50)")]
        public string Cron { get; set; }
    }
}
