USE [MARITECH_ORX];
GO

IF OBJECT_ID(N'dbo.shipment_workflow_details', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.shipment_workflow_details
    (
        shipment_id varchar(20) NOT NULL,
        shipment_mode varchar(50) NULL,
        container_type varchar(50) NULL,
        number_of_containers int NULL,
        gross_weight numeric(20, 3) NULL,
        volume numeric(20, 3) NULL,
        dangerous_goods varchar(10) NULL,
        temperature_controlled bit NULL,
        selected_services_json nvarchar(max) NULL,
        created_at datetime2 NOT NULL CONSTRAINT DF_shipment_workflow_details_created_at DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_shipment_workflow_details PRIMARY KEY (shipment_id),
        CONSTRAINT fk_shipment_workflow_details_shipment
            FOREIGN KEY (shipment_id) REFERENCES dbo.shipments(shipment_id) ON DELETE CASCADE
    );
END
GO
