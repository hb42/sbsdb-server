import { Column, Entity, PrimaryColumn } from "typeorm";
import { Spielwiese } from "../interfaces/spielwiese";

@Entity("SBS_SPIELWIESE")
export class SbsSpielwiese implements Spielwiese {
  @PrimaryColumn("number", {
    name: "ID",
    nullable: false,
    primary: true,
    generated: true,
  })
  public id: string;

  @Column("varchar2", {
    name: "TEXT",
    nullable: false,
    length: 50,
  })
  public text: string;

}
