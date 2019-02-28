import { Column, Entity, Index, JoinColumn, ManyToOne, OneToMany, PrimaryColumn } from "typeorm";
import { SBS_AP_ADR } from "./SBS_AP_ADR";
import { SBS_AP_SW } from "./SBS_AP_SW";
import { SBS_APKLASSE } from "./SBS_APKLASSE";
import { SBS_APSTATISTIK } from "./SBS_APSTATISTIK";
import { SBS_HW } from "./SBS_HW";
import { SBS_OE } from "./SBS_OE";
import { SBS_SEGMENT } from "./SBS_SEGMENT";
import { SBS_TT_ISSUE } from "./SBS_TT_ISSUE";

@Entity("SBS_AP", {schema: "sbsdb"})
@Index("sbsap_index2", ["TCP"])
@Index("sbsap_index1", ["AP_NAME"])
@Index("FK916B726A31F69485", ["aPKLASSE_INDEX"])
@Index("FK916B726A5CA9D44E", ["sTANDORT_INDEX"])
@Index("FK916B726A1B5248DB", ["aPSTATISTIK_INDEX"])
@Index("FK916B726AE6978689", ["oE_INDEX"])
@Index("FK916B726ABF666AF", ["sEGMENT_INDEX"])
export class SBS_AP {

    @PrimaryColumn("bigint", { 
        nullable: false,
        primary: true,
        name: "AP_INDEX",
        })
    public AP_INDEX: string;
        
    @Column("varchar", { 
        nullable: false,
        length: 50,
        name: "AP_NAME",
        })
    public AP_NAME: string;
        
    @Column("longtext", { 
        nullable: true,
        default: () => "'NULL'",
        name: "BEMERKUNG",
        })
    public BEMERKUNG: string | null;
        
    @Column("varchar", { 
        nullable: false,
        length: 55,
        name: "BEZEICHNUNG",
        })
    public BEZEICHNUNG: string;
        
    @Column("bigint", { 
        nullable: false,
        name: "TCP",
        })
    public TCP: string;
        
    @ManyToOne((type) => SBS_APKLASSE, (SBS_APKLASSE) => SBS_APKLASSE.sBS_APs, { onDelete: "RESTRICT", onUpdate: "RESTRICT" })
    @JoinColumn({ name: "APKLASSE_INDEX"})
    public aPKLASSE_INDEX: SBS_APKLASSE | null;

    @ManyToOne((type) => SBS_APSTATISTIK, (SBS_APSTATISTIK) => SBS_APSTATISTIK.sBS_APs, { onDelete: "RESTRICT", onUpdate: "RESTRICT" })
    @JoinColumn({ name: "APSTATISTIK_INDEX"})
    public aPSTATISTIK_INDEX: SBS_APSTATISTIK | null;

    @ManyToOne((type) => SBS_OE, (SBS_OE) => SBS_OE.sBS_APs2, { onDelete: "RESTRICT", onUpdate: "RESTRICT" })
    @JoinColumn({ name: "OE_INDEX"})
    public oE_INDEX: SBS_OE | null;

    @ManyToOne((type) => SBS_OE, (SBS_OE) => SBS_OE.sBS_APs, { onDelete: "RESTRICT", onUpdate: "RESTRICT" })
    @JoinColumn({ name: "STANDORT_INDEX"})
    public sTANDORT_INDEX: SBS_OE | null;

    @ManyToOne((type) => SBS_SEGMENT, (SBS_SEGMENT) => SBS_SEGMENT.sBS_APs, { onDelete: "RESTRICT", onUpdate: "RESTRICT" })
    @JoinColumn({ name: "SEGMENT_INDEX"})
    public sEGMENT_INDEX: SBS_SEGMENT | null;

    @OneToMany((type) => SBS_HW, (SBS_HW) => SBS_HW.aP_INDEX, { onDelete: "RESTRICT" , onUpdate: "RESTRICT" })
    public sBS_HWs: SBS_HW[];
    
    @OneToMany((type) => SBS_AP_ADR, (SBS_AP_ADR) => SBS_AP_ADR.aP_INDEX, { onDelete: "RESTRICT" , onUpdate: "RESTRICT" })
    public sBS_AP_ADRs: SBS_AP_ADR[];
    
    @OneToMany((type) => SBS_TT_ISSUE, (SBS_TT_ISSUE) => SBS_TT_ISSUE.aP_INDEX, { onDelete: "RESTRICT" , onUpdate: "RESTRICT" })
    public sBS_TT_ISSUEs: SBS_TT_ISSUE[];
    
    @OneToMany((type) => SBS_AP_SW, (SBS_AP_SW) => SBS_AP_SW.aP_INDEX, { onDelete: "RESTRICT" , onUpdate: "RESTRICT" })
    public sBS_AP_SWs: SBS_AP_SW[];
    
}
