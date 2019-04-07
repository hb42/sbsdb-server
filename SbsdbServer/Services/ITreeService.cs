using System.Collections.Generic;
using hb.SbsdbServer.ViewModel;

namespace hb.SbsdbServer.Services {
  public interface ITreeService {
    OeTreeItem GetOeTree();
    List<OeTreeItem> GetBstTree();
    IEnumerable<object> GetVlanTree();
  }
}
