namespace hb.SbsdbServer {
    public static class Const {
        public const string WEBSERVICE_PATH = "ws";

        // Controller path
        public const string API_PATH = WEBSERVICE_PATH + "/[controller]/[action]";

        // Bitmask fuer das Flag "fremde Hardware"
        public const byte FREMDE_HW = 0b_0000_0001;

        // App-Startpfad (nur Linux/MacOS)
        public const string BASE_URL = "/791/sbsdb";
        
        // Pfad fuer Server-Events
        public const string NOTIFICATION_PATH = "/" + WEBSERVICE_PATH + "/notification";
    }
}
