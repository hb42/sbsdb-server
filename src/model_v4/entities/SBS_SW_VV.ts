import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_SW } from "./SBS_SW";


@Entity("SBS_SW_VV",{schema:"sbsdb"})
@Index("sbsswvv_index1",["DATUM",])
@Index("FKA7B9C2603E4C6525",["sW_INDEX",])
export class SBS_SW_VV {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"SWVV_INDEX"
        })
    SWVV_INDEX:string;
        

    @Column("date",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"DATUM"
        })
    DATUM:string | null;
        

    @Column("longtext",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"DOKLINK"
        })
    DOKLINK:string | null;
        

    @Column("bit",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"KUNDEN_DATEN"
        })
    KUNDEN_DATEN: | null;
        

    @Column("bit",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"MA_DATEN"
        })
    MA_DATEN: | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"VERSION"
        })
    VERSION:string | null;
        

   
    @ManyToOne(type=>SBS_SW, SBS_SW=>SBS_SW.sBS_SW_VVs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'SW_INDEX'})
    sW_INDEX:SBS_SW | null;

}
