namespace hb.SbsdbServer.Model.ViewModel {
    public class TagChange {
        public long ApId { get; set; }
        public long? TagId { get; set; } // null => delete
        public long? ApTagId { get; set; } // null => new
        public string Text { get; set; }
    }
}
