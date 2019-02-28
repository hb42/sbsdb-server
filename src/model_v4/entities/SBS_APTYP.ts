import { Column, Entity, Index, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_ADRTYP } from "./SBS_ADRTYP";
import { SBS_APKLASSE } from "./SBS_APKLASSE";
import { SBS_APSTATISTIK } from "./SBS_APSTATISTIK";
import { SBS_HWTYP } from "./SBS_HWTYP";


@Entity("SBS_APTYP",{schema:"sbsdb"})
@Index("APTYP",["APTYP",],{unique:true})
@Index("sbsaptyp_index1",["LFD_NR",])
export class SBS_APTYP {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"APTYP_INDEX"
        })
    APTYP_INDEX:string;
        

    @Column("varchar",{ 
        nullable:false,
        unique: true,
        length:50,
        name:"APTYP"
        })
    APTYP:string;
        

    @Column("bigint",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"FLAG"
        })
    FLAG:string | null;
        

    @Column("bigint",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"LFD_NR"
        })
    LFD_NR:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:200,
        default: () => "'NULL'",
        name:"PARAM"
        })
    PARAM:string | null;
        

   
    @OneToMany(type=>SBS_APKLASSE, SBS_APKLASSE=>SBS_APKLASSE.aPTYP_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_APKLASSEs:SBS_APKLASSE[];
    

   
    @OneToMany(type=>SBS_APSTATISTIK, SBS_APSTATISTIK=>SBS_APSTATISTIK.aPTYP_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_APSTATISTIKs:SBS_APSTATISTIK[];
    

   
    @OneToMany(type=>SBS_ADRTYP, SBS_ADRTYP=>SBS_ADRTYP.aPTYP_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_ADRTYPs:SBS_ADRTYP[];
    

   
    @OneToMany(type=>SBS_HWTYP, SBS_HWTYP=>SBS_HWTYP.aPTYP_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_HWTYPs:SBS_HWTYP[];
    
}
