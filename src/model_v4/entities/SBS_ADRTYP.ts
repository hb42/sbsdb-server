import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_AP_ADR } from "./SBS_AP_ADR";
import { SBS_APTYP } from "./SBS_APTYP";


@Entity("SBS_ADRTYP",{schema:"sbsdb"})
@Index("sbsadrtyp_index1",["ADR_TYP",])
@Index("FK2FB5A0379E420B01",["aPTYP_INDEX",])
export class SBS_ADRTYP {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"ADR_INDEX"
        })
    ADR_INDEX:string;


    @Column("varchar",{
        nullable:false,
        length:50,
        name:"ADR_TYP"
        })
    ADR_TYP:string;


    @Column("bigint",{
        nullable:true,
        default: () => "'NULL'",
        name:"FLAG"
        })
    FLAG:string | null;


    @Column("varchar",{
        nullable:true,
        length:200,
        default: () => "'NULL'",
        name:"PARAM"
        })
    PARAM:string | null;



    @ManyToOne(type=>SBS_APTYP, SBS_APTYP=>SBS_APTYP.sBS_ADRTYPs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'APTYP_INDEX'})
    aPTYP_INDEX:SBS_APTYP | null;



    @OneToMany(type=>SBS_AP_ADR, SBS_AP_ADR=>SBS_AP_ADR.aDR_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_AP_ADRs:SBS_AP_ADR[];

}
