using System.Linq;
using hb.SbsdbServer.sbsdbv4.model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace hb.SbsdbServer.Model.Repositories {

  public class TreeRepository: ITreeRepository {

    // TODO v4
    private readonly Sbsdbv4Context dbContext;

    public TreeRepository(Sbsdbv4Context context) {
      dbContext = context;
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
      return bst.Where(b => b.ParentOe == 0 && b.OeIndex == 0);
    }

    /*
     * Flacher OE-Baum
     */
    public IEnumerable<object> GetBstTree() {
      // TODO hier koennten alle benoetigten Daten via Select geholt werden
      var bst = dbContext.SbsOe
        .Where(b => b.Ap > 0);
      return bst.ToList();
    }

  }
}
