﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hb.SbsdbServer.Model.ViewModel {
  public class Tag {
    public long ApTagId { get; set; }
    public string Text { get; set; }
    public long TagId { get; set; }
    public string Bezeichnung { get; set; }
    public long Flag { get; set; }
    public string Param { get; set; }
    public long AptypId { get; set; }

  }
}
