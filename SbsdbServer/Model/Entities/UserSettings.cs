namespace hb.SbsdbServer.Model.Entities
{
    public class UserSettings
    {
        public long Id { get; set; }
        public string Userid { get; set; }
        
        public string Settings { get; set; } // CLOB
    }
}
