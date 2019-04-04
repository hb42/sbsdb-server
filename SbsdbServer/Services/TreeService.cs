using System;
using hb.SbsdbServer.Model.Repositories;
using System.Collections;
using System.Collections.Generic;
using hb.SbsdbServer.ViewModel;

namespace hb.SbsdbServer.Services {
  public class TreeService: ITreeService {

    private readonly ITreeRepository treeRepository;

    public TreeService(ITreeRepository repo) {
      treeRepository = repo;
    }

    /*
     * Hierarchischer OE-Baum
     */
    public List<OeTreeItem> GetOeTree() {
      // TODO Daten vorbbereiten
      return treeRepository.GetOeTree();
    }

    /*
     * Flacher OE-Baum
     */
    public IEnumerable<object> GetBstTree() {
      // TODO Daten vorbbereiten
      return treeRepository.GetBstTree();
    }

  }
}
