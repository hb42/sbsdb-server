using System.Collections.Generic;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
    public class TreeService : ITreeService {
        private readonly ITreeRepository _treeRepository;

        public TreeService(ITreeRepository repo) {
            _treeRepository = repo;
        }

        /*
         * Hierarchischer OE-Baum
         */
        public List<OeTreeItem> GetOeTree() {
            // TODO Daten vorbereiten
            return _treeRepository.GetOeTree();
        }

        /*
         * Flacher OE-Baum
         */
        public List<OeTreeItem> GetBstTree() {
            // TODO Daten vorbereiten
            return _treeRepository.GetBstTree();
        }

        public IEnumerable<object> GetVlanTree() {
            return _treeRepository.GetVlans();
        }
    }
}
