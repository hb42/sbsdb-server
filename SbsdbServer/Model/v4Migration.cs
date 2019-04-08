using hb.SbsdbServer.sbsdbv4.model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hb.SbsdbServer.Model {

  public class v4Migration {

    private readonly Sbsdbv4Context v4dbContext;
    private readonly SbsdbContext v5dbContext;
    private readonly ILogger LOG;

    public v4Migration(Sbsdbv4Context sbsdbv4, SbsdbContext sbsdbv5, ILogger<v4Migration> log) {
      v4dbContext = sbsdbv4;
      v5dbContext = sbsdbv5;
      LOG = log;
    }

    public string Run() {
      LOG.LogDebug("Start migrating v4-DB");

      return "not yet implemented";
    }

  }
}
