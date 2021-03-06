namespace hb.SbsdbServer.Services {
    public interface IConfigService {
        object GetVersion();
        string GetConfig(string config);
        string SetConfig(string config, string value);
        void DelConfig(string config);
    }
}
