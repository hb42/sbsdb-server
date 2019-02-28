import { Column, Entity, Index, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_TT_ISSUE } from "./SBS_TT_ISSUE";


@Entity("SBS_TT_KATEGORIE",{schema:"sbsdb"})
@Index("sbsttkategorie_index1",["KATEGORIE",])
@Index("sbsttkategorie_index2",["FLAG",])
export class SBS_TT_KATEGORIE {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"KATEGORIE_INDEX"
        })
    KATEGORIE_INDEX:string;
        

    @Column("bigint",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"FLAG"
        })
    FLAG:string | null;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"KATEGORIE"
        })
    KATEGORIE:string;
        

   
    @OneToMany(type=>SBS_TT_ISSUE, SBS_TT_ISSUE=>SBS_TT_ISSUE.kATEGORIE_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_TT_ISSUEs:SBS_TT_ISSUE[];
    
}
