import { Column, Entity, Index, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_PREFS } from "./SBS_PREFS";
import { SBS_TT_ISSUE } from "./SBS_TT_ISSUE";


@Entity("SBS_USER",{schema:"sbsdb"})
@Index("USER_ID",["USER_ID",],{unique:true})
@Index("sbsuser_index1",["ROLLE",])
export class SBS_USER {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"USER_INDEX"
        })
    USER_INDEX:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"NACHNAME"
        })
    NACHNAME:string | null;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"PASSWORD"
        })
    PASSWORD:string;
        

    @Column("bigint",{ 
        nullable:false,
        name:"ROLLE"
        })
    ROLLE:string;
        

    @Column("varchar",{ 
        nullable:false,
        unique: true,
        length:50,
        name:"USER_ID"
        })
    USER_ID:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"VORNAME"
        })
    VORNAME:string | null;
        

   
    @OneToMany(type=>SBS_TT_ISSUE, SBS_TT_ISSUE=>SBS_TT_ISSUE.uSER_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_TT_ISSUEs:SBS_TT_ISSUE[];
    

   
    @OneToMany(type=>SBS_PREFS, SBS_PREFS=>SBS_PREFS.uSER_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_PREFSs:SBS_PREFS[];
    
}
