using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public interface IUserRepository {
        UserSession GetUser(string uid);
        void SetUser(UserSession user);
        void DeleteUser(long id);
    }
}
