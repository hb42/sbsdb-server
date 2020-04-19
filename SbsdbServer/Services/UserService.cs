using System;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public class UserService : IUserService {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository repo) {
            _userRepository = repo;
        }

        public UserSession GetUser(string uid) {
            if (string.IsNullOrWhiteSpace(uid)) throw new Exception("UID for get user settings cannot be empty!");
            var u = _userRepository.GetUser(uid);
            return u;
        }

        public void SetUser(string uid, UserSession user) {
            if (string.IsNullOrWhiteSpace(uid)) throw new Exception("UID for set user settings cannot be empty!");
            uid = uid.ToUpper();
            if (user.UID.Equals(uid))
                _userRepository.SetUser(user); // throws on error
            else
                throw new Exception($"User {uid} want's to change settings for user {user.UID}. This is not allowed!");
        }

        public void DeleteUser(long id) {
            _userRepository.DeleteUser(id);
        }
    }
}
