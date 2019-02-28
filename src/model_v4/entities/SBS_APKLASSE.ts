import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_AP } from "./SBS_AP";
import { SBS_APTYP } from "./SBS_APTYP";
import { SBS_EXTPROG } from "./SBS_EXTPROG";


@Entity("SBS_APKLASSE",{schema:"sbsdb"})
@Index("FK872BB9CF9E420B01",["aPTYP_INDEX",])
export class SBS_APKLASSE {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"APKLASSE_INDEX"
        })
    APKLASSE_INDEX:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"APKLASSE"
        })
    APKLASSE:string;
        

    @Column("bigint",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"FLAG"
        })
    FLAG:string | null;
        

   
    @ManyToOne(type=>SBS_APTYP, SBS_APTYP=>SBS_APTYP.sBS_APKLASSEs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'APTYP_INDEX'})
    aPTYP_INDEX:SBS_APTYP | null;


   
    @OneToMany(type=>SBS_EXTPROG, SBS_EXTPROG=>SBS_EXTPROG.aPKLASSE_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_EXTPROGs:SBS_EXTPROG[];
    

   
    @OneToMany(type=>SBS_AP, SBS_AP=>SBS_AP.aPKLASSE_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_APs:SBS_AP[];
    
}
