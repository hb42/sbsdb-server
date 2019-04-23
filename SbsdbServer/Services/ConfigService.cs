using hb.Common.Version;
using hb.SbsdbServer.Model.Repositories;

namespace hb.SbsdbServer.Services {
    public class ConfigService: IConfigService {

        private readonly VersionResource _versionResource;
        private readonly IConfigRepository _configRepository;
        
        public ConfigService(VersionResource version, IConfigRepository repo) {
            _versionResource = version;
            _configRepository = repo;
        }
        
        public object GetVersion() {
            return _versionResource.Package();
        }

        public string GetConfig(string config) {
            return _configRepository.GetConfig(config);
        }

        public string SetConfig(string config, string value) {
            return _configRepository.SetConfig(config, value);
        }

        public void DelConfig(string config) {
            _configRepository.DelConfig(config);
        }
    }
}
