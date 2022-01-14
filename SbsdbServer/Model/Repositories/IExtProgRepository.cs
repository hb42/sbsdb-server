using System.Collections.Generic;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Model.Repositories; 

public interface IExtProgRepository {
    public List<ExtProg> GetAll();
}
