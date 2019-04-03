using System;
using hb.SbsdbServer.Model.Entities;

namespace hb.SbsdbServer.Services {
  public interface IUserService {
    User GetUser(string uid);
    void SetUser(string uid, User user);
    void DeleteUser(long id);
  }
}
