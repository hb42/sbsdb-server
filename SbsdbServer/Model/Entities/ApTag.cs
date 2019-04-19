namespace hb.SbsdbServer.Model.Entities {
    public class ApTag {
        public long Id { get; set; }
        public string Text { get; set; }
        public long TagtypId { get; set; }
        public long ApId { get; set; }

        public virtual Ap Ap { get; set; }
        public virtual Tagtyp Tagtyp { get; set; }
    }
}
