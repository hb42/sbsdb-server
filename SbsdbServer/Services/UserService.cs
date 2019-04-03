﻿using System;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.Repositories;

namespace hb.SbsdbServer.Services {
  public class UserService: IUserService {

    private readonly IUserRepository userRepository;

    public UserService(IUserRepository repo) {
      userRepository = repo;
    }

    public User GetUser(string uid) {
      if (string.IsNullOrWhiteSpace(uid)) {
        throw new Exception("UID for get user settings cannot be empty!");
      }
      User u = userRepository.GetUser(uid);
      u.UID = u.UID.ToUpper();
      return u;
    }

    public void SetUser(string uid, User user) {
      if (string.IsNullOrWhiteSpace(uid)) {
        throw new Exception("UID for set user settings cannot be empty!");
      }
      uid = uid.ToUpper();
      user.UID = user.UID.ToUpper();
      if (user.UID.Equals(uid)) {
        userRepository.SetUser(user);  // throws on error
      } else {
        throw new Exception($"User {uid} want's to change settings for user {user.UID}. This is not allowed!");
      }
    }

    public void DeleteUser(long id) {
      userRepository.DeleteUser(id);
    }
  }
}