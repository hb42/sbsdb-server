using System.Collections.Generic;
using hb.SbsdbServer.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
  public interface ITreeRepository {
    OeTreeItem GetOeTree();
    List<OeTreeItem> GetBstTree();
    IEnumerable<object> GetVlans();
  }
}
