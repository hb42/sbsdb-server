import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_ADRTYP } from "./SBS_ADRTYP";
import { SBS_AP } from "./SBS_AP";


@Entity("SBS_AP_ADR",{schema:"sbsdb"})
@Index("sbsapadr_index1",["ADR_TEXT",])
@Index("FK3064593A7E2B9C7B",["aP_INDEX",])
@Index("FK3064593A807B9F48",["aDR_INDEX",])
export class SBS_AP_ADR {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"APADR_INDEX"
        })
    APADR_INDEX:string;


    @Column("varchar",{
        nullable:false,
        length:50,
        name:"ADR_TEXT"
        })
    ADR_TEXT:string;



    @ManyToOne(type=>SBS_ADRTYP, SBS_ADRTYP=>SBS_ADRTYP.sBS_AP_ADRs,{  nullable:false,onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'ADR_INDEX'})
    aDR_INDEX:SBS_ADRTYP | null;



    @ManyToOne(type=>SBS_AP, SBS_AP=>SBS_AP.sBS_AP_ADRs,{  nullable:false,onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'AP_INDEX'})
    aP_INDEX:SBS_AP | null;

}
