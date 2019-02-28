import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_AP } from "./SBS_AP";
import { SBS_HWSHIFT } from "./SBS_HWSHIFT";
import { SBS_KONFIG } from "./SBS_KONFIG";


@Entity("SBS_HW",{schema:"sbsdb"})
@Index("sbshw_index1",["SER_NR",])
@Index("FK916B734A7E2B9C7B",["aP_INDEX",])
@Index("FK916B734AF6F4CB51",["kONFIG_INDEX",])
export class SBS_HW {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"HW_INDEX"
        })
    HW_INDEX:string;
        

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
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"INV_NR"
        })
    INV_NR:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"MAC"
        })
    MAC:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"NETBOOTGUID"
        })
    NETBOOTGUID:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:1,
        default: () => "'NULL'",
        name:"PRI"
        })
    PRI:string | null;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"SER_NR"
        })
    SER_NR:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:200,
        default: () => "'NULL'",
        name:"WARTUNG_BEM"
        })
    WARTUNG_BEM:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"WARTUNG_FA"
        })
    WARTUNG_FA:string | null;
        

   
    @ManyToOne(type=>SBS_AP, SBS_AP=>SBS_AP.sBS_HWs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'AP_INDEX'})
    aP_INDEX:SBS_AP | null;


   
    @ManyToOne(type=>SBS_KONFIG, SBS_KONFIG=>SBS_KONFIG.sBS_HWs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'KONFIG_INDEX'})
    kONFIG_INDEX:SBS_KONFIG | null;


   
    @OneToMany(type=>SBS_HWSHIFT, SBS_HWSHIFT=>SBS_HWSHIFT.hW_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_HWSHIFTs:SBS_HWSHIFT[];
    
}
