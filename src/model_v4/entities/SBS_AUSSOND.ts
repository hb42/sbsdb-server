import { Column, Entity, Index, PrimaryColumn } from "typeorm";


@Entity("SBS_AUSSOND",{schema:"sbsdb"})
@Index("sbsaussond_index2",["AUSS_DAT",])
@Index("sbsaussond_index1",["SER_NR",])
export class SBS_AUSSOND {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"HW_INDEX"
        })
    HW_INDEX:string;
        

    @Column("date",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"ANSCH_DAT"
        })
    ANSCH_DAT:string | null;
        

    @Column("double",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"ANSCH_WERT"
        })
    ANSCH_WERT:number | null;
        

    @Column("date",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"AUSS_DAT"
        })
    AUSS_DAT:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"AUSS_GRUND"
        })
    AUSS_GRUND:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"INV_NR"
        })
    INV_NR:string | null;
        

    @Column("bigint",{ 
        nullable:true,
        default: () => "'NULL'",
        name:"KONFIG_INDEX"
        })
    KONFIG_INDEX:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"MAC"
        })
    MAC:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"NETBOOTGUID"
        })
    NETBOOTGUID:string | null;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"SER_NR"
        })
    SER_NR:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:200,
        default: () => "'NULL'",
        name:"WARTUNG_BEM"
        })
    WARTUNG_BEM:string | null;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"WARTUNG_FA"
        })
    WARTUNG_FA:string | null;
        
}
