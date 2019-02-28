import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_HW } from "./SBS_HW";
import { SBS_HWTYP } from "./SBS_HWTYP";


@Entity("SBS_KONFIG",{schema:"sbsdb"})
@Index("sbskonfig_index2",["BEZEICHNUNG",])
@Index("sbskonfig_index1",["HERSTELLER",])
@Index("FK415F10F55EEAD941",["hWTYP_INDEX",])
export class SBS_KONFIG {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"KONFIG_INDEX"
        })
    KONFIG_INDEX:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"BEZEICHNUNG"
        })
    BEZEICHNUNG:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"HD"
        })
    HD:string | null;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"HERSTELLER"
        })
    HERSTELLER:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"PROZESSOR"
        })
    PROZESSOR:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"RAM"
        })
    RAM:string | null;
        

    @Column("longtext",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"SONST"
        })
    SONST:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"VIDEO"
        })
    VIDEO:string | null;
        

   
    @ManyToOne(type=>SBS_HWTYP, SBS_HWTYP=>SBS_HWTYP.sBS_KONFIGs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'HWTYP_INDEX'})
    hWTYP_INDEX:SBS_HWTYP | null;


   
    @OneToMany(type=>SBS_HW, SBS_HW=>SBS_HW.kONFIG_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_HWs:SBS_HW[];
    
}
