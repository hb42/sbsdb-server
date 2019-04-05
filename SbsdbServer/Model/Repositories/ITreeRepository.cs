using System.Collections.Generic;
using hb.SbsdbServer.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
  public interface ITreeRepository {
    IEnumerable<object> GetOeTree();
    IEnumerable<object> GetBstTree();
  }
}
