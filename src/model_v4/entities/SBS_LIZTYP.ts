import { Column, Entity, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_SW } from "./SBS_SW";


@Entity("SBS_LIZTYP",{schema:"sbsdb"})
export class SBS_LIZTYP {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"LIZTYP_INDEX"
        })
    LIZTYP_INDEX:string;
        

    @Column("bigint",{ 
        nullable:false,
        name:"ABRECHNUNG"
        })
    ABRECHNUNG:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"LIZENZIERUNG"
        })
    LIZENZIERUNG:string;
        

   
    @OneToMany(type=>SBS_SW, SBS_SW=>SBS_SW.lIZTYP_INDEX,{ onDelete: 'RESTRICT' ,onUpdate: 'RESTRICT' })
    sBS_SWs:SBS_SW[];
    
}
