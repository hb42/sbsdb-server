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

        public decimal Id { get; set; }
        public decimal? Ap { get; set; }
        public string Betriebsstelle { get; set; }
        public decimal Bst { get; set; }
        public string Fax { get; set; }
        public string Oeff { get; set; }
        public string Tel { get; set; }
        public decimal OeId { get; set; }
        public decimal? AdresseId { get; set; }

        public virtual Adresse Adresse { get; set; }
        public virtual Oe OeNavigation { get; set; }
        public virtual ICollection<Ap> ApOe { get; set; }
        public virtual ICollection<Ap> ApOeIdVerOeNavigation { get; set; }
        public virtual ICollection<Oe> InverseOeNavigation { get; set; }
    }
}
