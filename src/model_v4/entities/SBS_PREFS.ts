import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_USER } from "./SBS_USER";


@Entity("SBS_PREFS",{schema:"sbsdb"})
@Index("sbsprefs_index1",["PREFERENCE",])
@Index("FKA78CD275A65A7C33",["uSER_INDEX",])
export class SBS_PREFS {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"PREFS_INDEX"
        })
    PREFS_INDEX:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"PREFERENCE"
        })
    PREFERENCE:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:200,
        default: () => "'NULL'",
        name:"TEXT"
        })
    TEXT:string | null;
        

   
    @ManyToOne(type=>SBS_USER, SBS_USER=>SBS_USER.sBS_PREFSs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'USER_INDEX'})
    uSER_INDEX:SBS_USER | null;

}
