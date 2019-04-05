using System.Collections.Generic;
using hb.SbsdbServer.ViewModel;

namespace hb.SbsdbServer.Services {
  public interface ITreeService {
    IEnumerable<object> GetOeTree();
    IEnumerable<object> GetBstTree();
  }
}
