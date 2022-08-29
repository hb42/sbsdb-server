using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
    public class UserRepository : IUserRepository {
        private readonly SbsdbContext _dbContext;
        private readonly ILogger<UserRepository> _log;

        public UserRepository(SbsdbContext context, ILogger<UserRepository> log) {
            _dbContext = context;
            _log = log;
        }

        public void DeleteUser(long id) {
            var user = _dbContext.UserSettings.Find(id);
            if (user != null) {
                _dbContext.Remove(user);
                _dbContext.SaveChanges();
            } else {
                _log.LogError("Error in DeleteUser() User {Id} ist nicht vorhanden!", id);
            }
        }

        public UserSession GetUser(string uid) {
            UserSettings user = GetUserRecord(uid);
            return new UserSession {
                UID = user.Userid.ToUpper(),
                Settings = user.Settings
            };
        }

        public void SetUser(UserSession user) {
            UserSettings settings = GetUserRecord(user.UID);
            settings.Settings = user.Settings;
            _dbContext.Update(settings);
            _dbContext.SaveChanges(); // throws on error
        }
        
        private UserSettings GetUserRecord(string uid) {
            var user = _dbContext.UserSettings.FirstOrDefault(u => u.Userid == uid);
            if (user == null) {
                user = new UserSettings {
                    Userid = uid.ToUpper(),
                    Settings = ""
                };
                _dbContext.UserSettings.Add(user);
                _dbContext.SaveChanges();
            }
            return user;
        }
        
    }
}
