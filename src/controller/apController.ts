import { NextFunction, Request, Response } from "express";
import { getRepository, Repository, SelectQueryBuilder } from "typeorm";

import { SBS_AP } from "../model_v4";

export class ApController {

  private apRepository: Repository<SBS_AP>;
  private apQuery: SelectQueryBuilder<SBS_AP>;

  constructor() {
    this.apRepository = getRepository(SBS_AP, "sbsdb_v4");

    this.apQuery = this.apRepository.createQueryBuilder("ap")
        .leftJoinAndSelect("ap.sBS_HWs", "hw")
        .leftJoinAndSelect("hw.kONFIG_INDEX", "konfig")
        .leftJoinAndSelect("ap.sBS_AP_ADRs", "adr")
        .leftJoinAndSelect("ap.sBS_TT_ISSUEs", "tt")
        .leftJoinAndSelect("ap.aPKLASSE_INDEX", "apklasse")
        .leftJoinAndSelect("ap.aPSTATISTIK_INDEX", "apstat")
        .leftJoinAndSelect("ap.oE_INDEX", "oe")
        .leftJoinAndSelect("ap.sTANDORT_INDEX", "standort")
        .leftJoinAndSelect("ap.sEGMENT_INDEX", "segment");
  }

  public async all(request: Request, response: Response, next: NextFunction) {
    //   return this.apRepository.find();
    console.info(this.apQuery.getSql());
    return this.apQuery.getMany();
  }

  public async one(request: Request, response: Response, next: NextFunction) {
    return this.apRepository.findOne(request.params.id);
  }

  public async search(request: Request, response: Response, next: NextFunction) {
    // console.info(request.params.query.toUpperCase());
    // TODO hier scheint die :name-Syntax nicht zu funktionieren
    const q: SelectQueryBuilder<SBS_AP> = this.apQuery
        .where("upper(ap.AP_NAME) like '%" + request.params.query.toUpperCase() + "%'");
    console.info(q.getSql());
    return q.getMany();

  }

  /*
  async save(request: Request, response: Response, next: NextFunction) {
      return this.apRepository.save(request.body);
  }

  async remove(request: Request, response: Response, next: NextFunction) {
      let apToRemove = await this.apRepository.findOne(request.params.id);
      if (apToRemove) {
        await this.apRepository.remove(apToRemove);
      }
  }
  */
}
