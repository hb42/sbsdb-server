using hb.SbsdbServer.Model.Entities;

namespace hb.SbsdbServer.Model.Repositories {
  public interface IUserRepository {
    User GetUser(string UID);
    void SetUser(User user);
    void DeleteUser(long id);
  }
}
