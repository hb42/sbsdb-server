import { Column, Entity, Index, JoinColumn, ManyToOne, PrimaryColumn } from "typeorm";
import { SBS_AP } from "./SBS_AP";
import { SBS_TT_KATEGORIE } from "./SBS_TT_KATEGORIE";
import { SBS_USER } from "./SBS_USER";


@Entity("SBS_TT_ISSUE",{schema:"sbsdb"})
@Index("sbsttissue_index2",["PRIO",])
@Index("sbsttissue_index3",["CLOSE",])
@Index("sbsttissue_index1",["OPEN",])
@Index("FK3279D0357E2B9C7B",["aP_INDEX",])
@Index("FK3279D035A65A7C33",["uSER_INDEX",])
@Index("FK3279D035F95D3827",["kATEGORIE_INDEX",])
export class SBS_TT_ISSUE {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"TTISSUE_INDEX"
        })
    TTISSUE_INDEX:string;
        

    @Column("datetime",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"CLOSE"
        })
    CLOSE:Date | null;
        

    @Column("datetime",{ 
        nullable:false,
        name:"OPEN"
        })
    OPEN:Date;
        

    @Column("bigint",{ 
        nullable:false,
        name:"PRIO"
        })
    PRIO:string;
        

    @Column("longtext",{ 
        nullable:false,
        name:"TICKET"
        })
    TICKET:string;
        

   
    @ManyToOne(type=>SBS_AP, SBS_AP=>SBS_AP.sBS_TT_ISSUEs,{ onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'AP_INDEX'})
    aP_INDEX:SBS_AP | null;


   
    @ManyToOne(type=>SBS_TT_KATEGORIE, SBS_TT_KATEGORIE=>SBS_TT_KATEGORIE.sBS_TT_ISSUEs,{  nullable:false,onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'KATEGORIE_INDEX'})
    kATEGORIE_INDEX:SBS_TT_KATEGORIE | null;


   
    @ManyToOne(type=>SBS_USER, SBS_USER=>SBS_USER.sBS_TT_ISSUEs,{  nullable:false,onDelete: 'RESTRICT',onUpdate: 'RESTRICT' })
    @JoinColumn({ name:'USER_INDEX'})
    uSER_INDEX:SBS_USER | null;

}
