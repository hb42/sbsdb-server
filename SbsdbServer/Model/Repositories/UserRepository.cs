using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
  public class UserRepository: IUserRepository {

    private readonly SbsdbContext dbContext;

    public UserRepository(SbsdbContext context) {
      dbContext = context;
    }

    public void DeleteUser(long id) {
      UserSettings user = dbContext.UserSettings.Find(id);
      if (user != null) {  // TODO Fehler "User nicht gefunden" wird nicht zurueckgegeben
        dbContext.Remove(user);
        dbContext.SaveChanges();
      }
    }

    public UserSession GetUser(string UID) {
      UserSettings user = dbContext.UserSettings.FirstOrDefault(u => u.Userid == UID);
      if (user == null) {
        user = new UserSettings {
          Userid = UID,
          Settings = new UserSession {
            UID = UID
          }
        };
        dbContext.UserSettings.Add(user);
        dbContext.SaveChanges();
      }
      return user.Settings;
    }

    public void SetUser(UserSession user) {
      dbContext.Update(user);
      dbContext.SaveChanges();  // throws on error
    }
  }
}
