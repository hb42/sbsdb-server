import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_APTYP } from "./SBS_APTYP";
import { SBS_KONFIG } from "./SBS_KONFIG";


@Entity("SBS_HWTYP",{schema:"sbsdb"})
@Index("FKA71E96E19E420B01",["aPTYP_INDEX",])
export class SBS_HWTYP {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"HWTYP_INDEX"
        })
    HWTYP_INDEX:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"HWTYP"
        })
    HWTYP:string;
        

   
    @ManyToOne(type=>SBS_APTYP, SBS_APTYP=>SBS_APTYP.sBS_HWTYPs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'APTYP_INDEX'})
    aPTYP_INDEX:SBS_APTYP | null;


   
    @OneToMany(type=>SBS_KONFIG, SBS_KONFIG=>SBS_KONFIG.hWTYP_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_KONFIGs:SBS_KONFIG[];
    
}
