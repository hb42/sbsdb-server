import { NextFunction, Request, Response } from "express";
import { getManager, getRepository, Repository, SelectQueryBuilder } from "typeorm";

import { SbsSpielwiese } from "../model/entities/SbsSpielwiese";
import { Spielwiese } from "../model/interfaces/spielwiese";

export class SwController {

  private swRepository: Repository<SbsSpielwiese>;
  private swQuery: SelectQueryBuilder<SbsSpielwiese>;

  constructor() {
    this.swRepository = getRepository(SbsSpielwiese, "sbsdb");

    this.swQuery = this.swRepository.createQueryBuilder("sw");
        // .leftJoinAndSelect("ap.sBS_HWs", "hw")
        // .leftJoinAndSelect("hw.kONFIG_INDEX", "konfig")
        // .leftJoinAndSelect("ap.sBS_AP_ADRs", "adr")
        // .leftJoinAndSelect("ap.sBS_TT_ISSUEs", "tt")
        // .leftJoinAndSelect("ap.aPKLASSE_INDEX", "apklasse")
        // .leftJoinAndSelect("ap.aPSTATISTIK_INDEX", "apstat")
        // .leftJoinAndSelect("ap.oE_INDEX", "oe")
        // .leftJoinAndSelect("ap.sTANDORT_INDEX", "standort")
        // .leftJoinAndSelect("ap.sEGMENT_INDEX", "segment");
  }

  public async all(request: Request, response: Response, next: NextFunction) {
    //   return this.apRepository.find();
    console.info(this.swQuery.getSql());
    const rc: Spielwiese[] = await this.swQuery.getMany();
    return rc;
  }

  public async one(request: Request, response: Response, next: NextFunction) {
    return this.swRepository.findOne(request.params.id);
  }

  // public async search(request: Request, response: Response, next: NextFunction) {
  //   // !! Syntax fuer param mit LIKE !!
  //   const q: SelectQueryBuilder<SBS_AP> = this.apQuery
  //       .where("upper(ap.AP_NAME) like :qs", {qs: `%${ request.params.query.toUpperCase() }%`});
  //   console.info(q.getSql());
  //   return q.getMany();
  // }

  public async save(request: Request, response: Response, next: NextFunction) {
    const rec = new SbsSpielwiese();
    rec.text = request.params.txt;
    return this.swRepository.save(rec);
  }
  /*
  async remove(request: Request, response: Response, next: NextFunction) {
      let apToRemove = await this.apRepository.findOne(request.params.id);
      if (apToRemove) {
        await this.apRepository.remove(apToRemove);
      }
  }
  */
}
