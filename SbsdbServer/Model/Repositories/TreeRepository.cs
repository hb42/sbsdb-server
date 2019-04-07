using hb.SbsdbServer.sbsdbv4.model;
using hb.SbsdbServer.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace hb.SbsdbServer.Model.Repositories {

  public class TreeRepository: ITreeRepository {

    // TODO v4
    private readonly Sbsdbv4Context dbContext;
    private readonly ILogger<TreeRepository> LOG;

    public TreeRepository(Sbsdbv4Context context, ILogger<TreeRepository> log) {
      dbContext = context;
      LOG = log;
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
      List<OeTreeItem> bst = dbContext.SbsOe
        .Select(b => new OeTreeItem {
          OeIndex = b.OeIndex,
          ParentOe = b.ParentOe,
          Ap = b.Ap > 0,
          Betriebsstelle = b.Betriebsstelle,
          Bst = b.Bst,
          Fax = b.Fax,
          Oeff = b.Oeff,
          Tel = b.Tel,
          FilialeIndex = b.FilialeIndex,
          Hausnr = b.FilialeIndexNavigation.Hausnr,
          Ort = b.FilialeIndexNavigation.Ort,
          Plz = b.FilialeIndexNavigation.Plz,
          Strasse = b.FilialeIndexNavigation.Strasse,
          Leaf = b.InverseParentOeNavigation.Count == 0
        })
        .ToList();

      OeTreeItem root = bst.First(b => b.OeIndex == 0 && b.ParentOe == 0);
      List<OeTreeItem> children = bst
        .Where(b => b.ParentOe == 0 && b.OeIndex != 0)
        .OrderBy(b => b.Betriebsstelle)
        .ToList();
      return MakeOeTree(root, children, bst);
    }

    /*
     * Flacher OE-Baum
     */
    public List<OeTreeItem> GetBstTree() {
      List<OeTreeItem> bst = dbContext.SbsOe
        .Select(b => new OeTreeItem {
          OeIndex = b.OeIndex,
          ParentOe = b.ParentOe,
          Ap = b.Ap > 0,
          Betriebsstelle = b.Betriebsstelle,
          Bst = b.Bst,
          Fax = b.Fax,
          Oeff = b.Oeff,
          Tel = b.Tel,
          FilialeIndex = b.FilialeIndex,
          Hausnr = b.FilialeIndexNavigation.Hausnr,
          Ort = b.FilialeIndexNavigation.Ort,
          Plz = b.FilialeIndexNavigation.Plz,
          Strasse = b.FilialeIndexNavigation.Strasse,
          Leaf = b.InverseParentOeNavigation.Count == 0
        })
        .Where(b => b.Ap == true)
        .OrderBy(b => b.Betriebsstelle)
        .ToList();
      return bst;
    }

    public IEnumerable<object> GetVlans() {
      var vlan = dbContext.SbsSegment
        .Select(v => new {
          v.SegmentIndex,
          v.SegmentName,
          v.Tcp,
          v.Netmask
        })
        .ToList();

      return vlan;
    }

    // Hierarchischer Baum: root node
    private OeTreeItem MakeOeTree(OeTreeItem root, List<OeTreeItem> children, List<OeTreeItem> list) {
      //root.Children = new List<OeTreeItem>();
      foreach(OeTreeItem item in children) {
      //  item.Children = new List<OeTreeItem>();
        root.Children.Add(item);
        if (!item.Leaf) {
          AddOes(item, list);
        }
      }
      return root;
    }
    // Hierarchischer Baum: alles unterhalb root rekursiv zusammensetzen
    private void AddOes(OeTreeItem parent, List<OeTreeItem> list) {
      List<OeTreeItem> children = list.Where(b => b.ParentOe == parent.OeIndex)
        .OrderBy(b => b.Betriebsstelle).ToList();
      foreach (OeTreeItem item in children) {
     //   item.Children = new List<OeTreeItem>();
        parent.Children.Add(item);
        if (!item.Leaf) {
          AddOes(item, list);
        }
      }
    }

  }
}
