import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_AP } from "./SBS_AP";
import { SBS_APTYP } from "./SBS_APTYP";


@Entity("SBS_APSTATISTIK",{schema:"sbsdb"})
@Index("sbsapstatistik_index1",["SORT",])
@Index("sbsapstatistik_index2",["FLAG",])
@Index("FKDBC82E0E9E420B01",["aPTYP_INDEX",])
export class SBS_APSTATISTIK {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"APSTATISTIK_INDEX"
        })
    APSTATISTIK_INDEX:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"APSTATISTIK"
        })
    APSTATISTIK:string;
        

    @Column("bigint",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"FLAG"
        })
    FLAG:string | null;
        

    @Column("bigint",{ 
        nullable:false,
        name:"SORT"
        })
    SORT:string;
        

   
    @ManyToOne(type=>SBS_APTYP, SBS_APTYP=>SBS_APTYP.sBS_APSTATISTIKs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'APTYP_INDEX'})
    aPTYP_INDEX:SBS_APTYP | null;


   
    @OneToMany(type=>SBS_AP, SBS_AP=>SBS_AP.aPSTATISTIK_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_APs:SBS_AP[];
    
}
