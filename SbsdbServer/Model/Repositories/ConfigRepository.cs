using System.Linq;
using hb.SbsdbServer.Model.Entities;

namespace hb.SbsdbServer.Model.Repositories
{
    public class ConfigRepository: IConfigRepository {
        
        private readonly SbsdbContext _dbContext;

        public ConfigRepository(SbsdbContext context) {
            _dbContext = context;
        }
        
        public string GetConfig(string config) {
            return FindConfig(config).Value;
        }

        public string SetConfig(string config, string value) {
            var setting = FindConfig(config);
            setting.Value = value;
//            _dbContext.ProgramSettings.Add(setting);
            _dbContext.SaveChanges();
            return setting.Value;
        }

        public void DelConfig(string config) {
            var setting = FindConfig(config);
            _dbContext.ProgramSettings.Remove(setting);
            _dbContext.SaveChanges();
        }

        private ProgramSettings FindConfig(string config) {
            var setting = _dbContext.ProgramSettings.FirstOrDefault(s => s.Key.ToLower().Equals(config.ToLower()));
            if (setting == null) {
                setting = new ProgramSettings() {
                    Key = config.ToLower(),
                    Value = ""
                };
                _dbContext.ProgramSettings.Add(setting);
                _dbContext.SaveChanges();
            }
            return setting;
        }
    }
}