using hb.SbsdbServer.Model.Entities;

namespace hb.SbsdbServer.Model.Repositories {
  public interface IUserRepository {
    UserSession GetUser(string UID);
    void SetUser(UserSession user);
    void DeleteUser(long id);
  }
}
