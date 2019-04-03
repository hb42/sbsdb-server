using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Repositories {
  public interface ITreeRepository {
    IEnumerable<object> GetOeTree();
    IEnumerable<object> GetBstTree();
  }
}
