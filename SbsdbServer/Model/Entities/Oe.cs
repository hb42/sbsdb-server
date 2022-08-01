using System;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Entities
{
    public partial class Oe
    {
        public Oe()
        {
            ApOe = new HashSet<Ap>();
            ApOeIdVerOeNavigation = new HashSet<Ap>();
            InverseOeNavigation = new HashSet<Oe>();
        }

        public long Id { get; set; }
        public string Betriebsstelle { get; set; }
        public long Bst { get; set; }
        public string Tel { get; set; }
        public string Oeff { get; set; }
        public bool? Ap { get; set; }
        public long? OeId { get; set; }
        public long AdresseId { get; set; }

        public virtual Adresse Adresse { get; set; }
        public virtual Oe OeNavigation { get; set; }
        public virtual ICollection<Ap> ApOe { get; set; }
        public virtual ICollection<Ap> ApOeIdVerOeNavigation { get; set; }
        public virtual ICollection<Oe> InverseOeNavigation { get; set; }
    }
}
