import { Column, Entity, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_OE } from "./SBS_OE";
import { SBS_SEGMENT } from "./SBS_SEGMENT";


@Entity("SBS_FILIALE",{schema:"sbsdb"})
export class SBS_FILIALE {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"FILIALE_INDEX"
        })
    FILIALE_INDEX:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"HAUSNR"
        })
    HAUSNR:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"ORT"
        })
    ORT:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"PLZ"
        })
    PLZ:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"STRASSE"
        })
    STRASSE:string | null;
        

   
    @OneToMany(type=>SBS_OE, SBS_OE=>SBS_OE.fILIALE_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_OEs:SBS_OE[];
    

   
    @OneToMany(type=>SBS_SEGMENT, SBS_SEGMENT=>SBS_SEGMENT.fILIALE_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_SEGMENTs:SBS_SEGMENT[];
    
}
