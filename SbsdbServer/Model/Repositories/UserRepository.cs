using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public class UserRepository : IUserRepository {
        private readonly SbsdbContext _dbContext;

        public UserRepository(SbsdbContext context) {
            _dbContext = context;
        }

        public void DeleteUser(long id) {
            var user = _dbContext.UserSettings.Find(id);
            if (user != null) { // TODO Fehler "User nicht gefunden" wird nicht zurueckgegeben
                _dbContext.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public UserSession GetUser(string uid) {
            return GetUserRecord(uid).Settings;
        }

        public void SetUser(UserSession user) {
            UserSettings settings = GetUserRecord(user.UID);
            settings.Settings = user;
            _dbContext.Update(settings);
            _dbContext.SaveChanges(); // throws on error
        }
        
        private UserSettings GetUserRecord(string uid) {
            var user = _dbContext.UserSettings.FirstOrDefault(u => u.Userid == uid);
            if (user == null) {
                user = new UserSettings {
                    Userid = uid,
                    Settings = new UserSession {
                        UID = uid
                    }
                };
                _dbContext.UserSettings.Add(user);
                _dbContext.SaveChanges();
            }

            return user;
        }
        
    }
}
