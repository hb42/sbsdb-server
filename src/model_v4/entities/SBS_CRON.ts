import { Column, Entity, Index, PrimaryColumn } from "typeorm";


@Entity("SBS_CRON",{schema:"sbsdb"})
@Index("sbscron_index1",["CRON",])
export class SBS_CRON {

    @PrimaryColumn("varchar",{ 
        nullable:false,
        primary:true,
        length:20,
        name:"CRON_INDEX"
        })
    CRON_INDEX:string;
        

    @Column("varchar",{ 
        nullable:true,
        length:50,
        default: () => "'NULL'",
        name:"CRON"
        })
    CRON:string | null;
        
}
