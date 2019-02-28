/**
 * farc-server
 *
 * Date-Archiv-Server mit REST-API und Routinen fuers Einlesen der Endpunkte.
 *
 */

import { LoggerService } from "@hb42/lib-server";

import * as bodyParser from "body-parser";
import * as express from "express";
import "reflect-metadata";

import { Connection, ConnectionOptions, ConnectionOptionsReader, createConnection, } from "typeorm";
import * as v8 from "v8";
import { Routes } from "./routes";

/*
 Mehr Threads fuer fs und mongo(?) bereitstellen (default 4)
 -> http://stackoverflow.com/questions/22644328/when-is-the-thread-pool-used
 (alt.(?): --v8-pool-size=)
 */
process.env.UV_THREADPOOL_SIZE = "32";

// Standard-Logfile
LoggerService.init("resource/log4js-server.json");
const log = LoggerService.get("sbsdb-server.main");

log.info("sbsdb-server starting");
log.info(v8.getHeapStatistics());

let db: Connection;
const dbConnect = async () => {
  // mit benannter Connection muss der Name auch bei getRepository, etc. angegeben werden!
  const opt: ConnectionOptions =
                               // typeorm sucht ab node_modules, das kann beim Testen Aerger machen
            await new ConnectionOptionsReader({root: __dirname, configName: "ormconfig.json"}).get("sbsdb_v4");
  db = await createConnection(opt);
};

dbConnect();

// const apRepo = getRepository(SBS_AP, "sbsdb_v4");

   // create express app
const app = express();
app.use(bodyParser.json());

    // register express routes from defined application routes
Routes.forEach((route) => {
  (app as any)[route.method](route.route, (req: express.Request, res: express.Response, next: express.NextFunction) => {
    const result = (new (route.controller as any))[route.action](req, res, next);
    if (result instanceof Promise) {
      result.then((reslt) => reslt !== null && reslt !== undefined ? res.send(reslt) : undefined);
    } else if (result !== null && result !== undefined) {
      res.json(result);
    }
  });
});

    // setup express app here
    // ...

    // start express server
app.listen(3000);

/*  typeorm mit eigenem Confignamen muesste in etwas so funktionieren:
async function getConnectionOptions(connectionName: string = "default"): Promise<ConnectionOptions> {
  return new ConnectionOptionsReader({root: "./resource", configName: "ormconfig.json"}).get(connectionName);
}
const connection = createConnection(getConnectionOptions("sbsdb_my"));
*/

/*
const config = JSON.parse(fs.readFileSync(configFile, "utf8"));

// Services und config-data
const services = new ServiceHandler(config);

// FARC-Server
const farcserver = new Webserver(config.restAPIport, "farc", new FarcUserCheck(services, config.jwtTimeoutSec));
farcserver.setCorsOptions({origin: config.webapp, credentials: true});
farcserver.addApi(new FarcAPI(services));
farcserver.setSSE(sseNAME);
farcserver.setDebug(true);
farcserver.start();
services.setWebserver(farcserver);

// wird nur gebraucht, wenn kein IIS vorhanden
if (!config.IIS) {
// fake IIS
  const fakeIISserver = new Webserver(config.restAPIport + 42, "asp");
  const asp = new AspAPI();
  asp.setUser("v998dpve\\s0770007");
  asp.setWebservice({ farc: {server: config.restAPI, url: authURL} });
  fakeIISserver.setDebug(true);
  fakeIISserver.addApi(asp);
  fakeIISserver.setCorsOptions({origin: config.webapp, credentials: true});
  fakeIISserver.setStaticContent("");
  fakeIISserver.start();
}

// FARC-static Webapp-Server als Alternative zu electron
if (config.static) {
  const staticserver = new Webserver(config.restAPIport - 100, "farc-static");
  staticserver.setFaviconPath("./resource/favicon.ico");
  staticserver.setStaticContent("./static");
  staticserver.setStaticUrl("/");
  staticserver.start();
}
*/

// Beim Beenden aufraeumen
const runonexit = (evt: any) => {
  log.info("terminating on " + evt);
 // services.DATA.kill(evt);  // Prozess wird automatisch beendet?
 // services.db.mongo.close().then((mesg) => {
 //   log.info(mesg);
  LoggerService.shutdown();
  process.exit(0);
 // });
};

process.once("SIGINT", runonexit);
process.once("SIGTERM", runonexit);
process.once("SIGUSR2", runonexit);
process.once("exit", runonexit);
