namespace hb.SbsdbServer.Model.Repositories
{
    public interface IConfigRepository {
        string GetConfig(string config);
        string SetConfig(string config, string value);
        void DelConfig(string config);
    }
}