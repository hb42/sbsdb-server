import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_APKLASSE } from "./SBS_APKLASSE";


@Entity("SBS_EXTPROG",{schema:"sbsdb"})
@Index("FKBCD2838031F69485",["aPKLASSE_INDEX",])
export class SBS_EXTPROG {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"EXTPROG_INDEX"
        })
    EXTPROG_INDEX:string;
        

    @Column("varchar",{ 
        nullable:false,
        name:"EXTPROG"
        })
    EXTPROG:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"EXTPROG_NAME"
        })
    EXTPROG_NAME:string;
        

    @Column("varchar",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"EXTPROG_PAR"
        })
    EXTPROG_PAR:string | null;
        

    @Column("bigint",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"FLAG"
        })
    FLAG:string | null;
        

   
    @ManyToOne(type=>SBS_APKLASSE, SBS_APKLASSE=>SBS_APKLASSE.sBS_EXTPROGs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'APKLASSE_INDEX'})
    aPKLASSE_INDEX:SBS_APKLASSE | null;

}
