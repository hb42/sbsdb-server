using System.Collections.Generic;
using System.Linq;
using hb.SbsdbServer.Model.Entities;
using hb.SbsdbServer.Model.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace hb.SbsdbServer.Model.Repositories; 

public class ExtProgRepository: IExtProgRepository {
    private readonly SbsdbContext _dbContext;
    private readonly ILogger<ExtProgRepository> _log;

    public ExtProgRepository(SbsdbContext context, ILogger<ExtProgRepository> log) {
        _dbContext = context;
        _log = log;
    }

    public List<ExtProg> GetAll() {
        return Query(_dbContext.Extprog).ToList();
    }

    private IQueryable<ExtProg> Query(IQueryable<Extprog> ctx) {
        return ctx
            .AsNoTracking()
            .Select(ext => new ExtProg {
                Id = ext.Id,
                Bezeichnung = ext.Bezeichnung,
                Program = ext.ExtprogName,
                Param = ext.ExtprogPar,
                Flag = ext.Flag,
                AptypId = ext.AptypId
            });
    }
}
