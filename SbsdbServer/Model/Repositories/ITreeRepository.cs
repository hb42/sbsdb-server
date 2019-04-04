using System.Collections.Generic;
using hb.SbsdbServer.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
  public interface ITreeRepository {
    List<OeTreeItem> GetOeTree();
    IEnumerable<object> GetBstTree();
  }
}
