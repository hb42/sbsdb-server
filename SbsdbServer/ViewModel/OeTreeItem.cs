﻿using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.ViewModel {
  public class OeTreeItem {

    public long OeIndex { get; set; }
    public long ParentOe { get; set; }
    public bool Ap { get; set; }
//    [Column("BETRIEBSSTELLE", TypeName = "varchar(50)")]
    public string Betriebsstelle { get; set; }
//    [Column("BST", TypeName = "bigint(20)")]
    public long Bst { get; set; }
  //  [Column("FAX", TypeName = "varchar(50)")]
    public string Fax { get; set; }
  //  [Column("OEFF", TypeName = "longtext")]
    public string Oeff { get; set; }
  //  [Column("TEL", TypeName = "varchar(50)")]
    public string Tel { get; set; }

    public long? FilialeIndex { get; set; } // ??
  //  [Column("HAUSNR", TypeName = "varchar(50)")]
    public string Hausnr { get; set; }
  //  [Column("ORT", TypeName = "varchar(50)")]
    public string Ort { get; set; }
  //  [Column("PLZ", TypeName = "varchar(50)")]
    public string Plz { get; set; }
  //  [Column("STRASSE", TypeName = "varchar(50)")]
    public string Strasse { get; set; }

    public bool Leaf { get; set; }
    public List<OeTreeItem> Children = new List<OeTreeItem>();
  }
}