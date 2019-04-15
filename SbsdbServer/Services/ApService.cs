using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hb.SbsdbServer.Model.Repositories;
using hb.SbsdbServer.Model.ViewModel;

namespace hb.SbsdbServer.Services {
  public class ApService : IApService {

    private readonly IApRepository apRepository;

    public ApService(IApRepository repo) {
      apRepository = repo;
    }
    public Arbeitsplatz GetAp(long id) {
      return apRepository.GetAp(id);
    }

    public List<Arbeitsplatz> QueryAps(ApQuery query) {
      return apRepository.QueryAps(query);
    }
  }
}
