import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_SW } from "./SBS_SW";


@Entity("SBS_SW_BESTAND",{schema:"sbsdb"})
@Index("FKB99A81B33E4C6525",["sW_INDEX",])
export class SBS_SW_BESTAND {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"SWBESTAND_INDEX"
        })
    SWBESTAND_INDEX:string;
        

    @Column("date",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"ANSCH_DAT"
        })
    ANSCH_DAT:string | null;
        

    @Column("double",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"ANSCH_WERT"
        })
    ANSCH_WERT:number | null;
        

    @Column("bigint",{ 
        nullable:false,
        name:"ANZAHL"
        })
    ANZAHL:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"INV_NR"
        })
    INV_NR:string | null;
        

   
    @ManyToOne(type=>SBS_SW, SBS_SW=>SBS_SW.sBS_SW_BESTANDs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'SW_INDEX'})
    sW_INDEX:SBS_SW | null;

}
