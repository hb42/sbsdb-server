using hb.Common.Version;

namespace hb.SbsdbServer.Services {
    public class ConfigService: IConfigService {

        private readonly VersionResource _versionResource;
        
        public ConfigService(VersionResource version) {
            _versionResource = version;
        }
        
        public object GetVersion() {
            return _versionResource.Package();
        }
        
    }
}
