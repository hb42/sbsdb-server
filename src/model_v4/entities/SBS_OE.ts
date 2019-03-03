import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_AP } from "./SBS_AP";
import { SBS_FILIALE } from "./SBS_FILIALE";
import { SBS_SW } from "./SBS_SW";


@Entity("SBS_OE",{schema:"sbsdb"})
@Index("sbsoe_index2",["BETRIEBSSTELLE",])
@Index("sbsoe_index1",["BST",])
@Index("FK916B7411D06ED6EB",["pARENT_OE",])
@Index("FK916B7411427CC73D",["fILIALE_INDEX",])
export class SBS_OE {

    @PrimaryColumn("bigint",{
        nullable:false,
        primary:true,
        name:"OE_INDEX"
        })
    OE_INDEX:string;


    @Column("bigint",{
        nullable:true,
        default: () => "'NULL'",
        name:"AP"
        })
    AP:string | null;


    @Column("varchar",{
        nullable:false,
        length:50,
        name:"BETRIEBSSTELLE"
        })
    BETRIEBSSTELLE:string;


    @Column("bigint",{
        nullable:false,
        name:"BST"
        })
    BST:string;


    @Column("varchar",{
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"FAX"
        })
    FAX:string | null;


    @Column("longtext",{
        nullable:true,
        default: () => "'NULL'",
        name:"OEFF"
        })
    OEFF:string | null;


    @Column("varchar",{
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"TEL"
        })
    TEL:string | null;



    @ManyToOne(type=>SBS_OE, SBS_OE=>SBS_OE.sBS_OEs)
    @JoinColumn({ name:'PARENT_OE'})
    pARENT_OE:SBS_OE | null;



    @ManyToOne(type=>SBS_FILIALE, SBS_FILIALE=>SBS_FILIALE.sBS_OEs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'FILIALE_INDEX'})
    fILIALE_INDEX:SBS_FILIALE | null;



    @OneToMany(type=>SBS_OE, SBS_OE=>SBS_OE.pARENT_OE)
    sBS_OEs:SBS_OE[];



    @OneToMany(type=>SBS_AP, SBS_AP=>SBS_AP.sTANDORT_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_APs:SBS_AP[];



    @OneToMany(type=>SBS_AP, SBS_AP=>SBS_AP.oE_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_APs2:SBS_AP[];



    @OneToMany(type=>SBS_SW, SBS_SW=>SBS_SW.oE_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_SWs:SBS_SW[];

}
