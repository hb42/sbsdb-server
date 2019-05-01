using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories {
    public interface ITreeRepository {
        List<OeTreeItem> GetOeTree();
        List<OeTreeItem> GetBstTree();
        IEnumerable<object> GetVlans();
    }
}
