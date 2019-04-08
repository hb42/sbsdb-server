using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hb.SbsdbServer.Model.Entities {
  /*
   * Adresse einer OE
   */
  public class Adresse {

    [Required]
    public long Id { get; set; }

    [StringLength(50, ErrorMessage = "Haus-Nr. darf nicht länger als 50 Stellen sein.")]
    public string HausNr { get; set; }
    [StringLength(100, ErrorMessage = "Ortsname darf nicht länger als 50 Stellen sein.")]
    public string Ort { get; set; }
    [StringLength(50, ErrorMessage = "PLZ darf nicht länger als 50 Stellen sein.")]
    public string Plz { get; set; }
    [StringLength(100, ErrorMessage = "Strassenbezeichnung darf nicht länger als 100 Stellen sein.")]
    public string Strasse { get; set; }

  }
}
