using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public interface IUserService {
        UserSession GetUser(string uid);
        void SetUser(string uid, UserSession user);
        void DeleteUser(long id);
    }
}
