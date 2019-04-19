namespace hb.SbsdbServer.Model.Entities {
    public class Mac {
        public long Id { get; set; }
        public string Adresse { get; set; }
        public long? Ip { get; set; }
        public long HwId { get; set; }
        public long VlanId { get; set; }

        public virtual Hw Hw { get; set; }
        public virtual Vlan Vlan { get; set; }
    }
}
