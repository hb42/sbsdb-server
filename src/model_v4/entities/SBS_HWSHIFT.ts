import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_HW } from "./SBS_HW";


@Entity("SBS_HWSHIFT",{schema:"sbsdb"})
@Index("sbshwshift_index3",["DIRECTION",])
@Index("sbshwshift_index2",["HOST",])
@Index("sbshwshift_index1",["SHIFTDATE",])
@Index("FK59BE7F58C79F763B",["hW_INDEX",])
export class SBS_HWSHIFT {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"HWSHIFT_INDEX"
        })
    HWSHIFT_INDEX:string;
        

    @Column("bigint",{ 
        nullable:false,
        name:"AP_INDEX"
        })
    AP_INDEX:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"BETRIEBSSTELLE"
        })
    BETRIEBSSTELLE:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:55,
        default: () => "'NULL'",
        name:"BEZEICHNUNG"
        })
    BEZEICHNUNG:string | null;
        

    @Column("varchar",{ 
        nullable:false,
        length:2,
        name:"DIRECTION"
        })
    DIRECTION:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"HOST"
        })
    HOST:string | null;
        

    @Column("timestamp",{ 
        nullable:false,
        default: () => "'current_timestamp()'",
        name:"SHIFTDATE"
        })
    SHIFTDATE:Date;
        

   
    @ManyToOne(type=>SBS_HW, SBS_HW=>SBS_HW.sBS_HWSHIFTs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'HW_INDEX'})
    hW_INDEX:SBS_HW | null;

}
