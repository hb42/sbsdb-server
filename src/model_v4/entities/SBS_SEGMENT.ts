import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_AP } from "./SBS_AP";
import { SBS_FILIALE } from "./SBS_FILIALE";


@Entity("SBS_SEGMENT",{schema:"sbsdb"})
@Index("FK8044EDB8427CC73D",["fILIALE_INDEX",])
export class SBS_SEGMENT {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"SEGMENT_INDEX"
        })
    SEGMENT_INDEX:string;
        

    @Column("bigint",{ 
        nullable:false,
        name:"NETMASK"
        })
    NETMASK:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"SEGMENT_NAME"
        })
    SEGMENT_NAME:string;
        

    @Column("bigint",{ 
        nullable:false,
        name:"TCP"
        })
    TCP:string;
        

   
    @ManyToOne(type=>SBS_FILIALE, SBS_FILIALE=>SBS_FILIALE.sBS_SEGMENTs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'FILIALE_INDEX'})
    fILIALE_INDEX:SBS_FILIALE | null;


   
    @OneToMany(type=>SBS_AP, SBS_AP=>SBS_AP.sEGMENT_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_APs:SBS_AP[];
    
}
