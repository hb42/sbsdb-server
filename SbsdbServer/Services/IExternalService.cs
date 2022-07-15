namespace hb.SbsdbServer.Services; 

public interface IExternalService {
    public string ImportThinClientIPs();
    public string GetTcLogs();
}
