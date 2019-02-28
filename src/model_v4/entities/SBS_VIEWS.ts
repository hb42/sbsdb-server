import { Column, Entity, PrimaryColumn } from "typeorm";


@Entity("SBS_VIEWS",{schema:"sbsdb"})
export class SBS_VIEWS {

    @PrimaryColumn("bigint",{ 
        nullable:false,
        primary:true,
        name:"VIEW_INDEX"
        })
    VIEW_INDEX:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:50,
        name:"VIEW_NAME"
        })
    VIEW_NAME:string;
        

    @Column("longtext",{ 
        nullable:false,
        name:"VIEW_SQL"
        })
    VIEW_SQL:string;
        

    @Column("varchar",{ 
        nullable:false,
        length:1,
        name:"VIEW_TYPE"
        })
    VIEW_TYPE:string;
        
}
