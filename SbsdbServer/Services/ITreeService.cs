using System.Collections.Generic;
using hb.SbsdbServer.ViewModel;

namespace hb.SbsdbServer.Services {
  public interface ITreeService {
    List<OeTreeItem> GetOeTree();
    IEnumerable<object> GetBstTree();
  }
}
