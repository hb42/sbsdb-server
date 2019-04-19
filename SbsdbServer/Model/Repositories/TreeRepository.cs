using System.Collections.Generic;
using System.Linq;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories {
    public class TreeRepository : ITreeRepository {
        private readonly SbsdbContext _dbContext;
        private readonly ILogger<TreeRepository> _log;

        public TreeRepository(SbsdbContext context, ILogger<TreeRepository> log) {
            _dbContext = context;
            _log = log;
        }

        /*
         * Hierachischer OE-Baum
         */
        public OeTreeItem GetOeTree() {
            // vollstaendiger BST-Baum mit beliebiger Tiefe
            // Verlinkung via parent == parent.id, wobei id 0 der root-Knoten ist
            // => es muss ein root-Knoten existieren mit ID == 0 + parent == 0
            //    -> Name "Sparkasse" -> fix vorgeben! 

            // Hierarchische Abfragen machen mit EF nur Aerger. Das hier ist ein ueberschaubarer
            // Datenbestand => flache Abfrage und den Rest im Programm zusammenbauen.
            var bst = _dbContext.Oe
                .Select(b => new OeTreeItem {
                    Id = b.Id,
                    ParentId = b.OeId,
                    Ap = (bool) b.Ap,
                    Betriebsstelle = b.Betriebsstelle,
                    Bst = b.Bst,
                    Fax = b.Fax,
                    Oeff = b.Oeff,
                    Tel = b.Tel,
                    AdresseId = b.AdresseId,
                    Hausnr = b.Adresse.Hausnr,
                    Ort = b.Adresse.Ort,
                    Plz = b.Adresse.Plz,
                    Strasse = b.Adresse.Strasse,
                    Leaf = b.InverseOeNavigation.Count == 0
                })
                .ToList();

            var root = bst.First(b => b.Id == 0 && b.ParentId == 0);
            var children = 
                bst.Where(b => b.ParentId == 0 && b.Id != 0)
                .OrderBy(b => b.Betriebsstelle)
                .ToList();
            return MakeOeTree(root, children, bst);
        }

        /*
         * Flacher OE-Baum
         */
        public List<OeTreeItem> GetBstTree() {
            var bst = _dbContext.Oe
                .Select(b => new OeTreeItem {
                    Id = b.Id,
                    ParentId = b.OeId,
                    Ap = (bool) b.Ap,
                    Betriebsstelle = b.Betriebsstelle,
                    Bst = b.Bst,
                    Fax = b.Fax,
                    Oeff = b.Oeff,
                    Tel = b.Tel,
                    AdresseId = b.AdresseId,
                    Hausnr = b.Adresse.Hausnr,
                    Ort = b.Adresse.Ort,
                    Plz = b.Adresse.Plz,
                    Strasse = b.Adresse.Strasse,
                    Leaf = b.InverseOeNavigation.Count == 0
                })
                .Where(b => b.Ap)
                .OrderBy(b => b.Betriebsstelle)
                .ToList();
            return bst;
        }

        public IEnumerable<object> GetVlans() {
            var vlan = _dbContext.Vlan
                .Select(v => new {
                    v.Id,
                    v.Bezeichnung,
                    v.Ip,
                    v.Netmask
                })
                .ToList();

            return vlan;
        }

        // Hierarchischer Baum: root node
        private OeTreeItem MakeOeTree(OeTreeItem root, List<OeTreeItem> children, List<OeTreeItem> list) {
            //root.Children = new List<OeTreeItem>();
            foreach (var item in children) {
                //  item.Children = new List<OeTreeItem>();
                root.Children.Add(item);
                if (!item.Leaf) AddOes(item, list);
            }

            return root;
        }

        // Hierarchischer Baum: alles unterhalb root rekursiv zusammensetzen
        private void AddOes(OeTreeItem parent, List<OeTreeItem> list) {
            var children = 
                list.Where(b => b.ParentId == parent.Id)
                .OrderBy(b => b.Betriebsstelle).ToList();
            foreach (var item in children) {
                //   item.Children = new List<OeTreeItem>();
                parent.Children.Add(item);
                if (!item.Leaf) AddOes(item, list);
            }
        }
    }
}
