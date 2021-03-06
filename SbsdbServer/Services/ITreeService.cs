﻿using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public interface ITreeService {
        List<OeTreeItem> GetOeTree();
        List<OeTreeItem> GetBstTree();
        IEnumerable<object> GetVlanTree();
    }
}
