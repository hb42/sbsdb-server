using System.Collections.Generic;

namespace hb.SbsdbServer.Services {
  public interface ITreeService {
    IEnumerable<object> GetOeTree();
    IEnumerable<object> GetBstTree();
  }
}
