import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_SW } from "./SBS_SW";


@Entity("SBS_SW_OPDV",{schema:"sbsdb"})
@Index("sbsswopdv_index1",["DATUM",])
@Index("FKA04F6DD33E4C6525",["sW_INDEX",])
export class SBS_SW_OPDV {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"SWOPDV_INDEX"
        })
    SWOPDV_INDEX:string;
        

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
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"FREIGABE"
        })
    FREIGABE:string | null;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"RISIKOSTUFE"
        })
    RISIKOSTUFE:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"VERSION"
        })
    VERSION:string | null;
        

   
    @ManyToOne(type=>SBS_SW, SBS_SW=>SBS_SW.sBS_SW_OPDVs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'SW_INDEX'})
    sW_INDEX:SBS_SW | null;

}
