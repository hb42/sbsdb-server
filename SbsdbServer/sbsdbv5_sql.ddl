-- Generiert von Oracle SQL Developer Data Modeler 20.4.0.374.0801
--   am/um:        2022-06-07 17:24:13 MESZ
--   Site:      Oracle Database 12c
--   Typ:      Oracle Database 12c



DROP TABLE sbsdb_master.adresse CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.ap CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.ap_issue CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.ap_tag CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.apkategorie CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.aptyp CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.aussond CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.extprog CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.hw CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.hwhistory CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.hwkonfig CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.hwtyp CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.issuetyp CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.mac CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.oe CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.program_settings CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.tagtyp CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.user_settings CASCADE CONSTRAINTS;

DROP TABLE sbsdb_master.vlan CASCADE CONSTRAINTS;

-- predefined type, no DDL - MDSYS.SDO_GEOMETRY

-- predefined type, no DDL - XMLTYPE

CREATE TABLE sbsdb_master.adresse (
    id       NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_adresse_id NOT NULL,
    plz      VARCHAR2(50),
    ort      VARCHAR2(100),
    strasse  VARCHAR2(100),
    hausnr   VARCHAR2(50)
)
LOGGING;

ALTER TABLE sbsdb_master.adresse ADD CONSTRAINT adresse_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.ap (
    id            NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_ap_id NOT NULL,
    apname        VARCHAR2(50)
        CONSTRAINT nnc_ap_apname NOT NULL,
    bezeichnung   VARCHAR2(200)
        CONSTRAINT nnc_ap_bezeichnung NOT NULL,
    bemerkung     CLOB,
    oe_id         NUMBER(19)
        CONSTRAINT nnc_ap_oe_id NOT NULL,
    oe_id_ver_oe  NUMBER(19),
    aptyp_id      NUMBER(19)
        CONSTRAINT nnc_ap_aptyp_id NOT NULL
)
LOGGING;

COMMENT ON COLUMN sbsdb_master.ap.oe_id IS
    'Standort';

COMMENT ON COLUMN sbsdb_master.ap.oe_id_ver_oe IS
    'Verantwortliche OE';

CREATE INDEX sbsdb_master.ap_oe_id_idx ON
    sbsdb_master.ap (
        oe_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_oe_id_ver_oe_idx ON
    sbsdb_master.ap (
        oe_id_ver_oe
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_apname_idx ON
    sbsdb_master.ap (
        apname
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_aptyp_id_idx ON
    sbsdb_master.ap (
        aptyp_id
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.ap ADD CONSTRAINT ap_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.ap_issue (
    id           NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_ap_issue_id NOT NULL,
    issue        CLOB
        CONSTRAINT nnc_ap_issue_issue NOT NULL,
    prio         NUMBER(19)
        CONSTRAINT nnc_ap_issue_prio NOT NULL,
    open         DATE
        CONSTRAINT nnc_ap_issue_open NOT NULL,
    close        DATE,
    userid       VARCHAR2(20)
        CONSTRAINT nnc_ap_issue_userid NOT NULL,
    ap_id        NUMBER(19),
    issuetyp_id  NUMBER(19)
        CONSTRAINT nnc_ap_issue_issuetyp_id NOT NULL
)
LOGGING;

CREATE INDEX sbsdb_master.ap_issue_ap_id_idx ON
    sbsdb_master.ap_issue (
        ap_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_issue_userid_idx ON
    sbsdb_master.ap_issue (
        userid
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_issue_issuetyp_id_idx ON
    sbsdb_master.ap_issue (
        issuetyp_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_issue_open_idx ON
    sbsdb_master.ap_issue (
        open
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_issue_prio_idx ON
    sbsdb_master.ap_issue (
        prio
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_issue_close_idx ON
    sbsdb_master.ap_issue (
        close
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.ap_issue ADD CONSTRAINT ap_issue_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.ap_tag (
    id         NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_ap_tag_id NOT NULL,
    text       VARCHAR2(100),
    tagtyp_id  NUMBER(19)
        CONSTRAINT nnc_ap_tag_tagtyp_id NOT NULL,
    ap_id      NUMBER(19)
        CONSTRAINT nnc_ap_tag_ap_id NOT NULL
)
LOGGING;

CREATE INDEX sbsdb_master.ap_tag_ap_id_idx ON
    sbsdb_master.ap_tag (
        ap_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_tag_tagtyp_id_idx ON
    sbsdb_master.ap_tag (
        tagtyp_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.ap_tag_text_idx ON
    sbsdb_master.ap_tag (
        text
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.ap_tag ADD CONSTRAINT ap_tag_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.apkategorie (
    id           NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 0 MINVALUE 0 NOCACHE ORDER )
        CONSTRAINT nnc_apkategorie_id NOT NULL,
    bezeichnung  VARCHAR2(50)
        CONSTRAINT nnc_apkategorie_bezeichnung NOT NULL,
    flag         NUMBER(19)
)
LOGGING;

COMMENT ON COLUMN sbsdb_master.apkategorie.flag IS
    '1 == Peripherie';

ALTER TABLE sbsdb_master.apkategorie ADD CONSTRAINT apkategorie_pk PRIMARY KEY ( id );

ALTER TABLE sbsdb_master.apkategorie ADD CONSTRAINT apkategorie_bezeichnung_un UNIQUE ( bezeichnung );

CREATE TABLE sbsdb_master.aptyp (
    id              NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 0 MINVALUE 0 NOCACHE ORDER )
        CONSTRAINT nnc_aptyp_id NOT NULL,
    bezeichnung     VARCHAR2(50)
        CONSTRAINT nnc_aptyp_aptyp NOT NULL,
    flag            NUMBER(19),
    apkategorie_id  NUMBER(19) NOT NULL
)
LOGGING;

COMMENT ON COLUMN sbsdb_master.aptyp.flag IS
    '1 == fremde HW';

ALTER TABLE sbsdb_master.aptyp ADD CONSTRAINT aptyp_pk PRIMARY KEY ( id );

ALTER TABLE sbsdb_master.aptyp ADD CONSTRAINT aptyp_bezeichnung_un UNIQUE ( bezeichnung );

CREATE TABLE sbsdb_master.aussond (
    id           NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_aussond_id NOT NULL,
    ser_nr       VARCHAR2(50)
        CONSTRAINT nnc_aussond_ser_nr NOT NULL,
    ansch_dat    DATE,
    inv_nr       VARCHAR2(50),
    ansch_wert   NUMBER(19, 2),
    hwkonfig_id  NUMBER(19)
        CONSTRAINT nnc_aussond_hwkonfig_id NOT NULL,
    mac          VARCHAR2(50),
    smbiosguid   VARCHAR2(50),
    wartung_fa   VARCHAR2(50),
    bemerkung    CLOB,
    auss_dat     DATE,
    auss_grund   VARCHAR2(50),
    rewe         DATE
)
LOGGING;

CREATE INDEX sbsdb_master.aussond_ser_nr_idx ON
    sbsdb_master.aussond (
        ser_nr
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.aussond_auss_dat_idx ON
    sbsdb_master.aussond (
        auss_dat
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.aussond ADD CONSTRAINT aussond_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.extprog (
    id            NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_extprog_id NOT NULL,
    bezeichnung   VARCHAR2(255)
        CONSTRAINT nnc_extprog_extprog NOT NULL,
    extprog_name  VARCHAR2(50)
        CONSTRAINT nnc_extprog_extprog_name NOT NULL,
    extprog_par   VARCHAR2(255),
    flag          NUMBER(19),
    aptyp_id      NUMBER(19)
        CONSTRAINT nnc_extprog_aptyp_id NOT NULL
)
LOGGING;

ALTER TABLE sbsdb_master.extprog ADD CONSTRAINT extprog_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.hw (
    id           NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_hw_id NOT NULL,
    ser_nr       VARCHAR2(50)
        CONSTRAINT nnc_hw_ser_nr NOT NULL,
    ansch_dat    DATE,
    inv_nr       VARCHAR2(50),
    ansch_wert   NUMBER(19, 2),
    smbiosguid   VARCHAR2(50),
    wartung_fa   VARCHAR2(50),
    bemerkung    CLOB,
    pri          NUMBER(1)
        CONSTRAINT nnc_hw_pri NOT NULL,
    ap_id        NUMBER(19),
    hwkonfig_id  NUMBER(19) NOT NULL
)
LOGGING;

CREATE INDEX sbsdb_master.hw_ap_id_idx ON
    sbsdb_master.hw (
        ap_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.hw_hwkonfig_id_idx ON
    sbsdb_master.hw (
        hwkonfig_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.hw_ser_nr_idx ON
    sbsdb_master.hw (
        ser_nr
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.hw ADD CONSTRAINT hw_pk PRIMARY KEY ( id );

ALTER TABLE sbsdb_master.hw ADD CONSTRAINT hw_smbiosguid_un UNIQUE ( smbiosguid );

CREATE TABLE sbsdb_master.hwhistory (
    id              NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_hwhistory_id NOT NULL,
    ap_id           NUMBER(19)
        CONSTRAINT nnc_hwhistory_ap_id NOT NULL,
    betriebsstelle  VARCHAR2(100),
    ap_bezeichnung  VARCHAR2(200),
    direction       VARCHAR2(2)
        CONSTRAINT nnc_hwhistory_direction NOT NULL,
    apname          VARCHAR2(50),
    shiftdate       TIMESTAMP DEFAULT localtimestamp(6)
        CONSTRAINT nnc_hwhistory_shiftdate NOT NULL,
    hw_id           NUMBER(19) NOT NULL
)
LOGGING;

CREATE INDEX sbsdb_master.hwhistory_hw_id_idx ON
    sbsdb_master.hwhistory (
        hw_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.hwhistory_shiftdate_idx ON
    sbsdb_master.hwhistory (
        shiftdate
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.hwhistory_apname_idx ON
    sbsdb_master.hwhistory (
        apname
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.hwhistory_direction_idx ON
    sbsdb_master.hwhistory (
        direction
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.hwhistory ADD CONSTRAINT hwhistory_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.hwkonfig (
    id           NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_hwkonfig_id NOT NULL,
    bezeichnung  VARCHAR2(50)
        CONSTRAINT nnc_hwkonfig_bezeichnung NOT NULL,
    hersteller   VARCHAR2(50)
        CONSTRAINT nnc_hwkonfig_hersteller NOT NULL,
    hd           VARCHAR2(50),
    prozessor    VARCHAR2(50),
    ram          VARCHAR2(50),
    sonst        CLOB,
    video        VARCHAR2(50),
    hwtyp_id     NUMBER(19) NOT NULL
)
LOGGING;

CREATE INDEX sbsdb_master.hwkonfig_hwtyp_id_idx ON
    sbsdb_master.hwkonfig (
        hwtyp_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.hwkonfig_hersteller_idx ON
    sbsdb_master.hwkonfig (
        hersteller
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.hwkonfig_bezeichnung_idx ON
    sbsdb_master.hwkonfig (
        bezeichnung
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.hwkonfig ADD CONSTRAINT hwkonfig_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.hwtyp (
    id              NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_hwtyp_id NOT NULL,
    bezeichnung     VARCHAR2(50)
        CONSTRAINT nnc_hwtyp_hwtyp NOT NULL,
    flag            NUMBER(19),
    apkategorie_id  NUMBER(19) NOT NULL
)
LOGGING;

COMMENT ON COLUMN sbsdb_master.hwtyp.flag IS
    '1 == fremde HW';

CREATE INDEX sbsdb_master.hwtyp_aptyp_id_idx ON
    sbsdb_master.hwtyp (
        apkategorie_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.hwtyp_flag_idx ON
    sbsdb_master.hwtyp (
        flag
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.hwtyp ADD CONSTRAINT hwtyp_pk PRIMARY KEY ( id );

ALTER TABLE sbsdb_master.hwtyp ADD CONSTRAINT hwtyp_bezeichnung_un UNIQUE ( bezeichnung );

CREATE TABLE sbsdb_master.issuetyp (
    id           NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_issuetyp_id NOT NULL,
    bezeichnung  VARCHAR2(100)
        CONSTRAINT nnc_issuetyp_issuetyp NOT NULL,
    flag         NUMBER(19)
)
LOGGING;

CREATE INDEX sbsdb_master.issuetyp_flag_idx ON
    sbsdb_master.issuetyp (
        flag
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.issuetyp ADD CONSTRAINT issuetyp_pk PRIMARY KEY ( id );

ALTER TABLE sbsdb_master.issuetyp ADD CONSTRAINT issuetyp_bezeichnung_un UNIQUE ( bezeichnung );

CREATE TABLE sbsdb_master.mac (
    id       NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_mac_id NOT NULL,
    adresse  VARCHAR2(12)
        CONSTRAINT nnc_mac_mac NOT NULL,
    ip       NUMBER(19),
    hw_id    NUMBER(19)
        CONSTRAINT nnc_mac_hw_id NOT NULL,
    vlan_id  NUMBER(19)
)
LOGGING;

COMMENT ON COLUMN sbsdb_master.mac.ip IS
    '0 == DHCP';

CREATE INDEX sbsdb_master.mac_adresse_idx ON
    sbsdb_master.mac (
        adresse
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.mac ADD CONSTRAINT mac_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.oe (
    id              NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 0 MINVALUE 0 NOCACHE ORDER )
        CONSTRAINT nnc_oe_id NOT NULL,
    betriebsstelle  VARCHAR2(100)
        CONSTRAINT nnc_oe_betriebsstelle NOT NULL,
    bst             NUMBER(19)
        CONSTRAINT nnc_oe_bst NOT NULL,
    fax             VARCHAR2(50),
    tel             VARCHAR2(50),
    oeff            CLOB,
    ap              NUMBER(1),
    oe_id           NUMBER(19),
    adresse_id      NUMBER(19) NOT NULL
)
LOGGING;

COMMENT ON COLUMN sbsdb_master.oe.oe_id IS
    'NULL -> kein parent';

CREATE INDEX sbsdb_master.oe_adresse_id_idx ON
    sbsdb_master.oe (
        adresse_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.oe_oe_id_idx ON
    sbsdb_master.oe (
        oe_id
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.oe_bst_idx ON
    sbsdb_master.oe (
        bst
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.oe_betriebsstelle_idx ON
    sbsdb_master.oe (
        betriebsstelle
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.oe ADD CONSTRAINT oe_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.program_settings (
    id     NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_prefsv1_id NOT NULL,
    key    VARCHAR2(100)
        CONSTRAINT nnc_prefsv1_preference NOT NULL,
    value  CLOB
)
LOGGING;

CREATE INDEX sbsdb_master.program_settings_key_idx ON
    sbsdb_master.program_settings (
        key
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.program_settings ADD CONSTRAINT program_settings_pk PRIMARY KEY ( id );

ALTER TABLE sbsdb_master.program_settings ADD CONSTRAINT program_settings_key_un UNIQUE ( key );

CREATE TABLE sbsdb_master.tagtyp (
    id              NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_tagtyp_id NOT NULL,
    bezeichnung     VARCHAR2(50)
        CONSTRAINT nnc_tagtyp_tag_typ NOT NULL,
    flag            NUMBER(19) NOT NULL,
    param           VARCHAR2(200),
    apkategorie_id  NUMBER(19) NOT NULL
)
LOGGING;

COMMENT ON COLUMN sbsdb_master.tagtyp.flag IS
    '1 == kein param';

CREATE INDEX sbsdb_master.tagtyp_bezeichnung_idx ON
    sbsdb_master.tagtyp (
        bezeichnung
    ASC )
        LOGGING;

CREATE INDEX sbsdb_master.tagtyp_apkategorie_id_idx ON
    sbsdb_master.tagtyp (
        apkategorie_id
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.tagtyp ADD CONSTRAINT tagtyp_pk PRIMARY KEY ( id );

CREATE TABLE sbsdb_master.user_settings (
    id        NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 1 NOCACHE ORDER )
        CONSTRAINT nnc_prefs_id NOT NULL,
    userid    VARCHAR2(30) NOT NULL,
    settings  CLOB
)
LOGGING;

CREATE INDEX sbsdb_master.user_settings_userid_idx ON
    sbsdb_master.user_settings (
        userid
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.user_settings ADD CONSTRAINT user_settings_pk PRIMARY KEY ( id );

ALTER TABLE sbsdb_master.user_settings ADD CONSTRAINT user_settings_userid_un UNIQUE ( userid );

CREATE TABLE sbsdb_master.vlan (
    id           NUMBER(19)
        GENERATED BY DEFAULT AS IDENTITY ( START WITH 0 MINVALUE 0 NOCACHE ORDER )
        CONSTRAINT nnc_vlan_vlan_id NOT NULL,
    ip           NUMBER(19)
        CONSTRAINT nnc_vlan_ip NOT NULL,
    netmask      NUMBER(19)
        CONSTRAINT nnc_vlan_netmask NOT NULL,
    bezeichnung  VARCHAR2(100)
        CONSTRAINT nnc_vlan_vlan_name NOT NULL
)
LOGGING;

CREATE INDEX sbsdb_master.vlan_ip_idx ON
    sbsdb_master.vlan (
        ip
    ASC )
        LOGGING;

ALTER TABLE sbsdb_master.vlan ADD CONSTRAINT vlan_pk PRIMARY KEY ( id );

ALTER TABLE sbsdb_master.vlan ADD CONSTRAINT vlan_bezeichnung_un UNIQUE ( bezeichnung );

ALTER TABLE sbsdb_master.vlan ADD CONSTRAINT vlan_ip_un UNIQUE ( ip );

ALTER TABLE sbsdb_master.ap
    ADD CONSTRAINT ap_aptyp_fk FOREIGN KEY ( aptyp_id )
        REFERENCES sbsdb_master.aptyp ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.ap_issue
    ADD CONSTRAINT ap_issue_ap_fk FOREIGN KEY ( ap_id )
        REFERENCES sbsdb_master.ap ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.ap_issue
    ADD CONSTRAINT ap_issue_issuetyp_fk FOREIGN KEY ( issuetyp_id )
        REFERENCES sbsdb_master.issuetyp ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.ap
    ADD CONSTRAINT ap_oe_fk FOREIGN KEY ( oe_id )
        REFERENCES sbsdb_master.oe ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.ap
    ADD CONSTRAINT ap_oe_fk_ver_oe FOREIGN KEY ( oe_id_ver_oe )
        REFERENCES sbsdb_master.oe ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.ap_tag
    ADD CONSTRAINT ap_tag_ap_fk FOREIGN KEY ( ap_id )
        REFERENCES sbsdb_master.ap ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.ap_tag
    ADD CONSTRAINT ap_tag_tagtyp_fk FOREIGN KEY ( tagtyp_id )
        REFERENCES sbsdb_master.tagtyp ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.aptyp
    ADD CONSTRAINT aptyp_apkategorie_fk FOREIGN KEY ( apkategorie_id )
        REFERENCES sbsdb_master.apkategorie ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.aussond
    ADD CONSTRAINT aussond_hwkonfig_fk FOREIGN KEY ( hwkonfig_id )
        REFERENCES sbsdb_master.hwkonfig ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.extprog
    ADD CONSTRAINT extprog_aptyp_fk FOREIGN KEY ( aptyp_id )
        REFERENCES sbsdb_master.aptyp ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.hw
    ADD CONSTRAINT hw_ap_fk FOREIGN KEY ( ap_id )
        REFERENCES sbsdb_master.ap ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.hw
    ADD CONSTRAINT hw_hwkonfig_fk FOREIGN KEY ( hwkonfig_id )
        REFERENCES sbsdb_master.hwkonfig ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.hwhistory
    ADD CONSTRAINT hwhistory_hw_fk FOREIGN KEY ( hw_id )
        REFERENCES sbsdb_master.hw ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.hwkonfig
    ADD CONSTRAINT hwkonfig_hwtyp_fk FOREIGN KEY ( hwtyp_id )
        REFERENCES sbsdb_master.hwtyp ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.hwtyp
    ADD CONSTRAINT hwtyp_apkategorie_fk FOREIGN KEY ( apkategorie_id )
        REFERENCES sbsdb_master.apkategorie ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.mac
    ADD CONSTRAINT mac_hw_fk FOREIGN KEY ( hw_id )
        REFERENCES sbsdb_master.hw ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.mac
    ADD CONSTRAINT mac_vlan_fk FOREIGN KEY ( vlan_id )
        REFERENCES sbsdb_master.vlan ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.oe
    ADD CONSTRAINT oe_adresse_fk FOREIGN KEY ( adresse_id )
        REFERENCES sbsdb_master.adresse ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.oe
    ADD CONSTRAINT oe_oe_fk FOREIGN KEY ( oe_id )
        REFERENCES sbsdb_master.oe ( id )
    NOT DEFERRABLE;

ALTER TABLE sbsdb_master.tagtyp
    ADD CONSTRAINT tagtyp_apkategorie_fk FOREIGN KEY ( apkategorie_id )
        REFERENCES sbsdb_master.apkategorie ( id )
    NOT DEFERRABLE;



-- Zusammenfassungsbericht f�r Oracle SQL Developer Data Modeler: 
-- 
-- CREATE TABLE                            19
-- CREATE INDEX                            38
-- ALTER TABLE                             48
-- CREATE VIEW                              0
-- ALTER VIEW                               0
-- CREATE PACKAGE                           0
-- CREATE PACKAGE BODY                      0
-- CREATE PROCEDURE                         0
-- CREATE FUNCTION                          0
-- CREATE TRIGGER                           0
-- ALTER TRIGGER                            0
-- CREATE COLLECTION TYPE                   0
-- CREATE STRUCTURED TYPE                   0
-- CREATE STRUCTURED TYPE BODY              0
-- CREATE CLUSTER                           0
-- CREATE CONTEXT                           0
-- CREATE DATABASE                          0
-- CREATE DIMENSION                         0
-- CREATE DIRECTORY                         0
-- CREATE DISK GROUP                        0
-- CREATE ROLE                              0
-- CREATE ROLLBACK SEGMENT                  0
-- CREATE SEQUENCE                          0
-- CREATE MATERIALIZED VIEW                 0
-- CREATE MATERIALIZED VIEW LOG             0
-- CREATE SYNONYM                           0
-- CREATE TABLESPACE                        0
-- CREATE USER                              0
-- 
-- DROP TABLESPACE                          0
-- DROP DATABASE                            0
-- 
-- REDACTION POLICY                         0
-- TSDP POLICY                              0
-- 
-- ORDS DROP SCHEMA                         0
-- ORDS ENABLE SCHEMA                       0
-- ORDS ENABLE OBJECT                       0
-- 
-- ERRORS                                   0
-- WARNINGS                                 0
