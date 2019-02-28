import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_AP } from "./SBS_AP";
import { SBS_SW } from "./SBS_SW";


@Entity("SBS_AP_SW",{schema:"sbsdb"})
@Index("FKA6B8EC593E4C6525",["sW_INDEX",])
@Index("FKA6B8EC597E2B9C7B",["aP_INDEX",])
export class SBS_AP_SW {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"APSW_INDEX"
        })
    APSW_INDEX:string;
        

    @Column("bigint",{ 
        nullable:false,
        name:"ANZAHL"
        })
    ANZAHL:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"VER"
        })
    VER:string | null;
        

   
    @ManyToOne(type=>SBS_AP, SBS_AP=>SBS_AP.sBS_AP_SWs,{  nullable:false,onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'AP_INDEX'})
    aP_INDEX:SBS_AP | null;


   
    @ManyToOne(type=>SBS_SW, SBS_SW=>SBS_SW.sBS_AP_SWs,{  nullable:false,onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'SW_INDEX'})
    sW_INDEX:SBS_SW | null;

}
