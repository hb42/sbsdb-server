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
    public IEnumerable<object> GetOeTree() {

      // vollstaendiger BST-Baum mit beliebiger Tiefe
      // Verlinkung via parent == parent.id, wobei id 0 der root-Knoten ist
      // => es muss ein root-Knoten existieren mit ID == 0 + parent == 0
      //    -> Name "Sparkasse" -> fix vorgeben! 

      var bst = dbContext.SbsOe
        .Include(b => b.InverseParentOeNavigation)
        .ToList();
      // TODO Daten aus SbsFiliale muessen extra geholt werden, Include wuerde im Kreis laufen,
      //      waehrend Select nicht die Hierarchie wiedergeben wuerde.
      // nur das root-Element, alles weitere waere redunant
      //return bst.Where(b => b.ParentOe == 0 && b.OeIndex == 0);
      return BuildTree(bst.Where(b => b.ParentOe == 0 && b.OeIndex == 0));
    }

    /*
     * Flacher OE-Baum
     */
    public IEnumerable<object> GetBstTree() {
      var list = dbContext.SbsFiliale.ToList();
      var bst = dbContext.SbsOe
        .Where(b => b.Ap > 0);
      return BuildTree(bst.ToList());
    }

    private List<OeTreeItem> BuildTree(IEnumerable<SbsOe> oes) {
      LOG.LogDebug("BuildTree IN length=" + oes.Count());
      List<OeTreeItem> items = new List<OeTreeItem>();
      foreach (SbsOe oe in oes) {
        OeTreeItem item = new OeTreeItem {
          OeIndex = oe.OeIndex,
          Ap = oe.Ap > 0,
          Betriebsstelle = oe.Betriebsstelle,
          Bst = oe.Bst,
          Fax = oe.Fax,
          Oeff = oe.Oeff,
          Tel = oe.Tel,
        };
        if (oe.FilialeIndex != null) {
          item.FilialeIndex = oe.FilialeIndex;
          if (oe.FilialeIndexNavigation != null) {
            item.Hausnr = oe.FilialeIndexNavigation.Hausnr;
            item.Ort = oe.FilialeIndexNavigation.Ort;
            item.Plz = oe.FilialeIndexNavigation.Plz;
            item.Strasse = oe.FilialeIndexNavigation.Strasse;
          }
        } else {
          item.FilialeIndex = null;
        }
        item.Children = oe.InverseParentOeNavigation.Count > 0
          ? BuildTree(oe.InverseParentOeNavigation)  // Rekursion
          : new List<OeTreeItem>();
        items.Add(item);
      }
      return items;
    }

  }
}
