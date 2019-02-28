import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_AP_SW } from "./SBS_AP_SW";
import { SBS_LIZTYP } from "./SBS_LIZTYP";
import { SBS_OE } from "./SBS_OE";
import { SBS_SW_BESTAND } from "./SBS_SW_BESTAND";
import { SBS_SW_OPDV } from "./SBS_SW_OPDV";
import { SBS_SW_VV } from "./SBS_SW_VV";


@Entity("SBS_SW",{schema:"sbsdb"})
@Index("sbssw_index4",["EINSATZ",])
@Index("sbssw_index3",["SMS_PAKET",])
@Index("sbssw_index5",["VOLLZ_ERKL",])
@Index("sbssw_index2",["BEZEICHNUNG",])
@Index("sbssw_index1",["HERSTELLER",])
@Index("FK916B749F6C0A7979",["lIZTYP_INDEX",])
@Index("FK916B749FE6978689",["oE_INDEX",])
export class SBS_SW {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"SW_INDEX"
        })
    SW_INDEX:string;
        

    @Column("longtext",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"BESCHREIBUNG"
        })
    BESCHREIBUNG:string | null;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"BEZEICHNUNG"
        })
    BEZEICHNUNG:string;
        

    @Column("bit",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"EINSATZ"
        })
    EINSATZ: | null;
        

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
        name:"INST_ORT"
        })
    INST_ORT:string | null;
        

    @Column("double",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"LIZENZ_PA"
        })
    LIZENZ_PA:number | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:100,
        default: () => "'NULL'",
        name:"NUTZER"
        })
    NUTZER:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:20,
        default: () => "'NULL'",
        name:"SB_INTEGR"
        })
    SB_INTEGR:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:20,
        default: () => "'NULL'",
        name:"SB_NACHV"
        })
    SB_NACHV:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"SB_RECHTL"
        })
    SB_RECHTL:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:20,
        default: () => "'NULL'",
        name:"SB_VERFUEG"
        })
    SB_VERFUEG:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:20,
        default: () => "'NULL'",
        name:"SB_VERTR"
        })
    SB_VERTR:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"SMS_PAKET"
        })
    SMS_PAKET:string | null;
        

    @Column("bit",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"VOLLZ_ERKL"
        })
    VOLLZ_ERKL: | null;
        

   
    @ManyToOne(type=>SBS_LIZTYP, SBS_LIZTYP=>SBS_LIZTYP.sBS_SWs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'LIZTYP_INDEX'})
    lIZTYP_INDEX:SBS_LIZTYP | null;


   
    @ManyToOne(type=>SBS_OE, SBS_OE=>SBS_OE.sBS_SWs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'OE_INDEX'})
    oE_INDEX:SBS_OE | null;


   
    @OneToMany(type=>SBS_SW_OPDV, SBS_SW_OPDV=>SBS_SW_OPDV.sW_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_SW_OPDVs:SBS_SW_OPDV[];
    

   
    @OneToMany(type=>SBS_SW_BESTAND, SBS_SW_BESTAND=>SBS_SW_BESTAND.sW_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_SW_BESTANDs:SBS_SW_BESTAND[];
    

   
    @OneToMany(type=>SBS_AP_SW, SBS_AP_SW=>SBS_AP_SW.sW_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_AP_SWs:SBS_AP_SW[];
    

   
    @OneToMany(type=>SBS_SW_VV, SBS_SW_VV=>SBS_SW_VV.sW_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_SW_VVs:SBS_SW_VV[];
    
}
