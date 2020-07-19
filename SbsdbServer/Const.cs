namespace hb.SbsdbServer {
    public static class Const {
        // Controller path
        public const string API_PATH = "ws/[controller]/[action]";

        // Roles
        // Vollzugriff
//        public const string ROLE_ADMIN = "e077ggx-sbsdb-admin";
        // darf alles sehen, aber nichts aendern
//        public const string ROLE_READONLY = "e077ggx-sbsdb-readonly";
        // sieht APs + Remotezugriff
//        public const string ROLE_HOTLINE = "e077ggx-sbsdb-hotline";
// TODO fuer die Entwicklung erst mal mit vorhandenen Rollen arbeiten
        public const string ROLE_ADMIN = "e077ggx-791-it-service";
        public const string ROLE_READONLY = "e077ggx-sbsdb-readonly";
        public const string ROLE_HOTLINE = "e077ggx-000-institut";
        
        // Name in der Config-DB
        public const string AP_PAGE_SIZE = "ap.pagesize"; // *deprecated*
        
    }
}
