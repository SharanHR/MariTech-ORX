using System;
using System.Collections.Generic;
using MariTech.ORX.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace MariTech.ORX.Infrastructure.Persistence;

public partial class ORXDbContext : DbContext
{
    public ORXDbContext(DbContextOptions<ORXDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entities.Action> Actions { get; set; }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<CarrierService> CarrierServices { get; set; }

    public virtual DbSet<Container> Containers { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentExtraction> DocumentExtractions { get; set; }

    public virtual DbSet<EventImpact> EventImpacts { get; set; }

    public virtual DbSet<FreightQuote> FreightQuotes { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<NetworkEvent> NetworkEvents { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }

    public virtual DbSet<Port> Ports { get; set; }

    public virtual DbSet<Prediction> Predictions { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }

    public virtual DbSet<RiskAssessment> RiskAssessments { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }
    public virtual DbSet<ShipmentWorkflowDetail> ShipmentWorkflowDetails { get; set; }

    public virtual DbSet<TradeOrder> TradeOrders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vessel> Vessels { get; set; }

    public virtual DbSet<VoyageSchedule> VoyageSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Action>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK__actions__74EFC2170F9C8C68");

            entity.ToTable("actions");

            entity.Property(e => e.ActionId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("action_id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("action_type");
            entity.Property(e => e.Actor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("actor");
            entity.Property(e => e.RecommendationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("recommendation_id");
            entity.Property(e => e.RequestedAt)
                .HasColumnType("datetime")
                .HasColumnName("requested_at");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Target)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("target");

            entity.HasOne(d => d.ActorNavigation).WithMany(p => p.Actions)
                .HasForeignKey(d => d.Actor)
                .HasConstraintName("fk_actions_actor");

            entity.HasOne(d => d.Recommendation).WithMany(p => p.Actions)
                .HasForeignKey(d => d.RecommendationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_actions_recommendation");
        });

        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.CargoId).HasName("PK__cargo__982828C482396EB1");

            entity.ToTable("cargo");

            entity.HasIndex(e => e.TradeId, "idx_cargo_trade");

            entity.Property(e => e.CargoId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("cargo_id");
            entity.Property(e => e.Commodity)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("commodity");
            entity.Property(e => e.ContainerRequirement)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("container_requirement");
            entity.Property(e => e.DangerousGoods)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("dangerous_goods");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.GrossWeight)
                .HasColumnType("numeric(20, 3)")
                .HasColumnName("gross_weight");
            entity.Property(e => e.HsCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("hs_code");
            entity.Property(e => e.NetWeight)
                .HasColumnType("numeric(20, 3)")
                .HasColumnName("net_weight");
            entity.Property(e => e.PackagingType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("packaging_type");
            entity.Property(e => e.Quantity)
                .HasColumnType("numeric(20, 3)")
                .HasColumnName("quantity");
            entity.Property(e => e.QuantityUnit)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("quantity_unit");
            entity.Property(e => e.TradeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trade_id");
            entity.Property(e => e.Volume)
                .HasColumnType("numeric(20, 3)")
                .HasColumnName("volume");

            entity.HasOne(d => d.Trade).WithMany(p => p.Cargos)
                .HasForeignKey(d => d.TradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cargo_trade");
        });

        modelBuilder.Entity<CarrierService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__carrier___3E0DB8AF0393BC1B");

            entity.ToTable("carrier_services");

            entity.Property(e => e.ServiceId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("service_id");
            entity.Property(e => e.Carrier)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("carrier");
            entity.Property(e => e.Frequency)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("frequency");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("service_name");
            entity.Property(e => e.TradeLane)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("trade_lane");
        });

        modelBuilder.Entity<Container>(entity =>
        {
            entity.HasKey(e => e.ContainerId).HasName("PK__containe__48725BD9C2E6D37C");

            entity.ToTable("containers");

            entity.HasIndex(e => e.ContainerNumber, "UQ__containe__FF6C734D3D49E297").IsUnique();

            entity.HasIndex(e => e.ShipmentId, "idx_containers_shipment");

            entity.Property(e => e.ContainerId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("container_id");
            entity.Property(e => e.Availability)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("availability");
            entity.Property(e => e.Carrier)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("carrier");
            entity.Property(e => e.ContainerNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_number");
            entity.Property(e => e.ContainerType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("container_type");
            entity.Property(e => e.CurrentLocation)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("current_location");
            entity.Property(e => e.IsoCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("iso_code");
            entity.Property(e => e.SealNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("seal_number");
            entity.Property(e => e.ShipmentId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("shipment_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.Volume)
                .HasColumnType("numeric(20, 3)")
                .HasColumnName("volume");
            entity.Property(e => e.Weight)
                .HasColumnType("numeric(20, 3)")
                .HasColumnName("weight");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__document__9666E8AC28470E9B");

            entity.ToTable("documents");

            entity.HasIndex(e => e.ShipmentId, "idx_documents_shipment");

            entity.HasIndex(e => e.TradeId, "idx_documents_trade");

            entity.Property(e => e.DocumentId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("document_id");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("document_number");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("document_type");
            entity.Property(e => e.IssueDate).HasColumnName("issue_date");
            entity.Property(e => e.Issuer)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("issuer");
            entity.Property(e => e.ShipmentId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("shipment_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TradeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trade_id");
            entity.Property(e => e.Version).HasColumnName("version");

            entity.HasOne(d => d.Trade).WithMany(p => p.Documents)
                .HasForeignKey(d => d.TradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_documents_trade");
        });

        modelBuilder.Entity<DocumentExtraction>(entity =>
        {
            entity.HasKey(e => new { e.DocumentId, e.Field }).HasName("PK__document__46C4A3C277FD2579");

            entity.ToTable("document_extractions");

            entity.Property(e => e.DocumentId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("document_id");
            entity.Property(e => e.Field)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("field");
            entity.Property(e => e.Confidence)
                .HasColumnType("numeric(5, 4)")
                .HasColumnName("confidence");
            entity.Property(e => e.ExtractionModel)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("extraction_model");
            entity.Property(e => e.Value)
                .HasColumnType("text")
                .HasColumnName("value");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentExtractions)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_document_extractions_document");
        });

        modelBuilder.Entity<EventImpact>(entity =>
        {
            entity.HasKey(e => new { e.EventId, e.TradeId, e.ShipmentId }).HasName("PK__event_im__119E44F64CCE2C9C");

            entity.ToTable("event_impacts");

            entity.HasIndex(e => e.ShipmentId, "idx_event_impacts_shipment");

            entity.HasIndex(e => e.TradeId, "idx_event_impacts_trade");

            entity.Property(e => e.EventId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("event_id");
            entity.Property(e => e.TradeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trade_id");
            entity.Property(e => e.ShipmentId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("shipment_id");
            entity.Property(e => e.EstimatedCost)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("estimated_cost");
            entity.Property(e => e.EstimatedDelay)
                .HasColumnType("numeric(10, 2)")
                .HasColumnName("estimated_delay");
            entity.Property(e => e.ImpactScore)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("impact_score");
            entity.Property(e => e.ImpactType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("impact_type");
            entity.Property(e => e.Probability)
                .HasColumnType("numeric(5, 4)")
                .HasColumnName("probability");
            entity.Property(e => e.RecommendedAction)
                .HasColumnType("text")
                .HasColumnName("recommended_action");

            entity.HasOne(d => d.Event).WithMany(p => p.EventImpacts)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_impacts_event");

            entity.HasOne(d => d.Trade).WithMany(p => p.EventImpacts)
                .HasForeignKey(d => d.TradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_event_impacts_trade");
        });

        modelBuilder.Entity<FreightQuote>(entity =>
        {
            entity.HasKey(e => e.QuoteId).HasName("PK__freight___0D37DF0C96B0C73D");

            entity.ToTable("freight_quotes");

            entity.HasIndex(e => e.CustomerId, "idx_freight_quotes_customer");

            entity.Property(e => e.QuoteId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("quote_id");
            entity.Property(e => e.Baf)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("baf");
            entity.Property(e => e.BaseFreight)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("base_freight");
            entity.Property(e => e.Carrier)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("carrier");
            entity.Property(e => e.CongestionSurcharge)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("congestion_surcharge");
            entity.Property(e => e.ContainerType)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("container_type");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("customer_id");
            entity.Property(e => e.Destination)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("destination");
            entity.Property(e => e.EquipmentQuantity).HasColumnName("equipment_quantity");
            entity.Property(e => e.LocalCharges)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("local_charges");
            entity.Property(e => e.Origin)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("origin");
            entity.Property(e => e.OtherSurcharges)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("other_surcharges");
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("source");
            entity.Property(e => e.TotalCost)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("total_cost");
            entity.Property(e => e.ValidFrom).HasColumnName("valid_from");
            entity.Property(e => e.ValidUntil).HasColumnName("valid_until");

            entity.HasOne(d => d.Customer).WithMany(p => p.FreightQuotes)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_freight_quotes_customer");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__location__771831EA86B04914");

            entity.ToTable("locations");

            entity.HasIndex(e => e.Unlocode, "UQ__location__E7DB74356AD05EF3").IsUnique();

            entity.Property(e => e.LocationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("location_id");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.Latitude)
                .HasColumnType("numeric(10, 6)")
                .HasColumnName("latitude");
            entity.Property(e => e.LocationType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("location_type");
            entity.Property(e => e.Longitude)
                .HasColumnType("numeric(10, 6)")
                .HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Unlocode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("unlocode");
        });

        modelBuilder.Entity<NetworkEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__network___2370F727D36DC0D8");

            entity.ToTable("network_events");

            entity.Property(e => e.EventId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("event_id");
            entity.Property(e => e.Carrier)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("carrier");
            entity.Property(e => e.Confidence)
                .HasColumnType("numeric(5, 4)")
                .HasColumnName("confidence");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.EventType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("event_type");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Severity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("severity");
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("source");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.TradeLane)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("trade_lane");
            entity.Property(e => e.Vessel)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("vessel");

            entity.HasOne(d => d.VesselNavigation).WithMany(p => p.NetworkEvents)
                .HasForeignKey(d => d.Vessel)
                .HasConstraintName("fk_network_events_vessel");
        });

        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.HasKey(e => e.OrganisationId).HasName("PK__organisa__6CB3A4F23F24DE9C");

            entity.ToTable("organisations");

            entity.Property(e => e.OrganisationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("organisation_id");
            entity.Property(e => e.AnnualTradeVolume)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("annual_trade_volume");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.Industry)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("industry");
            entity.Property(e => e.LegalName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("legal_name");
            entity.Property(e => e.OrganisationType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("organisation_type");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TradeName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("trade_name");
        });

        modelBuilder.Entity<Port>(entity =>
        {
            entity.HasKey(e => e.PortId).HasName("PK__ports__E57CDF5BF6A3F567");

            entity.ToTable("ports");

            entity.HasIndex(e => e.LocationId, "idx_ports_location");

            entity.Property(e => e.PortId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("port_id");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("country");
            entity.Property(e => e.LocationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("location_id");
            entity.Property(e => e.PortAuthority)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("port_authority");

            entity.HasOne(d => d.Location).WithMany(p => p.Ports)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ports_location");
        });

        modelBuilder.Entity<Prediction>(entity =>
        {
            entity.HasKey(e => e.PredictionId).HasName("PK__predicti__F1AE77BF04CE6066");

            entity.ToTable("predictions");

            entity.Property(e => e.PredictionId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prediction_id");
            entity.Property(e => e.EntityId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("entity_id");
            entity.Property(e => e.EntityType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("entity_type");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.GeneratedAt)
                .HasColumnType("datetime")
                .HasColumnName("generated_at");
            entity.Property(e => e.ModelVersion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("model_version");
            entity.Property(e => e.PredictedValue)
                .HasColumnType("numeric(20, 6)")
                .HasColumnName("predicted_value");
            entity.Property(e => e.PredictionHorizon)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prediction_horizon");
            entity.Property(e => e.PredictionType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("prediction_type");
            entity.Property(e => e.Probability)
                .HasColumnType("numeric(5, 4)")
                .HasColumnName("probability");
        });

        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.RecommendationId).HasName("PK__recommen__BCB11F4F9F8B3553");

            entity.ToTable("recommendations");

            entity.HasIndex(e => e.TradeId, "idx_recommendations_trade");

            entity.Property(e => e.RecommendationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("recommendation_id");
            entity.Property(e => e.Action)
                .HasColumnType("text")
                .HasColumnName("action");
            entity.Property(e => e.Confidence)
                .HasColumnType("numeric(5, 4)")
                .HasColumnName("confidence");
            entity.Property(e => e.EstimatedCost)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("estimated_cost");
            entity.Property(e => e.ExpectedBenefit)
                .HasColumnType("text")
                .HasColumnName("expected_benefit");
            entity.Property(e => e.Reason)
                .HasColumnType("text")
                .HasColumnName("reason");
            entity.Property(e => e.RecommendationType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("recommendation_type");
            entity.Property(e => e.RiskReduction)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("risk_reduction");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TradeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trade_id");
            entity.Property(e => e.TriggerEvent)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("trigger_event");

            entity.HasOne(d => d.Trade).WithMany(p => p.Recommendations)
                .HasForeignKey(d => d.TradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_recommendations_trade");
        });

        modelBuilder.Entity<RiskAssessment>(entity =>
        {
            entity.HasKey(e => e.RiskId).HasName("PK__risk_ass__DF842621B227CA5E");

            entity.ToTable("risk_assessments");

            entity.HasIndex(e => e.ShipmentId, "idx_risk_assessments_shipment");

            entity.HasIndex(e => e.TradeId, "idx_risk_assessments_trade");

            entity.Property(e => e.RiskId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("risk_id");
            entity.Property(e => e.CalculatedAt)
                .HasColumnType("datetime")
                .HasColumnName("calculated_at");
            entity.Property(e => e.Evidence)
                .HasColumnType("text")
                .HasColumnName("evidence");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.Probability)
                .HasColumnType("numeric(5, 4)")
                .HasColumnName("probability");
            entity.Property(e => e.Reason)
                .HasColumnType("text")
                .HasColumnName("reason");
            entity.Property(e => e.RiskType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("risk_type");
            entity.Property(e => e.Score)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("score");
            entity.Property(e => e.Severity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("severity");
            entity.Property(e => e.ShipmentId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("shipment_id");
            entity.Property(e => e.TradeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trade_id");

            entity.HasOne(d => d.Trade).WithMany(p => p.RiskAssessments)
                .HasForeignKey(d => d.TradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_risk_assessments_trade");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PK__shipment__41466E59C42A8CC3");

            entity.ToTable("shipments");

            entity.HasIndex(e => e.BookingReference, "UQ__shipment__BADA455950F4A32C").IsUnique();

            entity.HasIndex(e => e.BookingReference, "UQ__shipment__BADA4559937CAAB2").IsUnique();

            entity.HasIndex(e => e.BookingReference, "UQ__shipment__BADA4559C251828B").IsUnique();

            entity.HasIndex(e => e.BookingReference, "UQ__shipment__BADA4559C5A88A68").IsUnique();

            entity.Property(e => e.ShipmentId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("shipment_id");
            entity.Property(e => e.BookingReference)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("booking_reference");
            entity.Property(e => e.Carrier)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("carrier");
            entity.Property(e => e.Consignee)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("consignee");
            entity.Property(e => e.CurrentEta)
                .HasColumnType("datetime")
                .HasColumnName("current_eta");
            entity.Property(e => e.CurrentEtd)
                .HasColumnType("datetime")
                .HasColumnName("current_etd");
            entity.Property(e => e.Destination)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("destination");
            entity.Property(e => e.Forwarder)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("forwarder");
            entity.Property(e => e.Origin)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("origin");
            entity.Property(e => e.PlannedEta)
                .HasColumnType("datetime")
                .HasColumnName("planned_eta");
            entity.Property(e => e.PlannedEtd)
                .HasColumnType("datetime")
                .HasColumnName("planned_etd");
            entity.Property(e => e.PortOfDischarge)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("port_of_discharge");
            entity.Property(e => e.PortOfLoading)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("port_of_loading");
            entity.Property(e => e.RiskScore)
                .HasColumnType("numeric(6, 2)")
                .HasColumnName("risk_score");
            entity.Property(e => e.Shipper)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("shipper");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TradeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trade_id");

            entity.HasOne(d => d.Trade).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.TradeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shipments_trade");
        });

        modelBuilder.Entity<ShipmentWorkflowDetail>(entity =>
        {
            entity.HasKey(e => e.ShipmentId);

            entity.ToTable("shipment_workflow_details");

            entity.Property(e => e.ShipmentId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("shipment_id");

            entity.Property(e => e.ShipmentMode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("shipment_mode");

            entity.Property(e => e.ContainerType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("container_type");

            entity.Property(e => e.NumberOfContainers)
                .HasColumnName("number_of_containers");

            entity.Property(e => e.GrossWeight)
                .HasColumnType("numeric(20, 3)")
                .HasColumnName("gross_weight");

            entity.Property(e => e.Volume)
                .HasColumnType("numeric(20, 3)")
                .HasColumnName("volume");

            entity.Property(e => e.DangerousGoods)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("dangerous_goods");

            entity.Property(e => e.TemperatureControlled)
                .HasColumnName("temperature_controlled");

            entity.Property(e => e.SelectedServicesJson)
                .HasColumnType("nvarchar(max)")
                .HasColumnName("selected_services_json");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime2")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Shipment)
                .WithOne()
                .HasForeignKey<ShipmentWorkflowDetail>(d => d.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_shipment_workflow_details_shipment");
        });

        modelBuilder.Entity<TradeOrder>(entity =>
        {
            entity.HasKey(e => e.TradeId).HasName("PK__trade_or__AAFF5BF77BAE24B6");

            entity.ToTable("trade_orders");

            entity.HasIndex(e => e.CustomerId, "idx_trade_orders_customer");

            entity.Property(e => e.TradeId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("trade_id");
            entity.Property(e => e.Buyer)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("buyer");
            entity.Property(e => e.Commodity)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("commodity");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("customer_id");
            entity.Property(e => e.DestinationCountry)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("destination_country");
            entity.Property(e => e.Incoterm)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("incoterm");
            entity.Property(e => e.OriginCountry)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("origin_country");
            entity.Property(e => e.PaymentTerms)
                .HasColumnType("text")
                .HasColumnName("payment_terms");
            entity.Property(e => e.PlannedShipDate).HasColumnName("planned_ship_date");
            entity.Property(e => e.RequiredDeliveryDate).HasColumnName("required_delivery_date");
            entity.Property(e => e.Seller)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("seller");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TradeType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("trade_type");
            entity.Property(e => e.TradeValue)
                .HasColumnType("numeric(20, 2)")
                .HasColumnName("trade_value");

            entity.HasOne(d => d.Customer).WithMany(p => p.TradeOrders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("fk_trade_orders_customer");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FB11646CE");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164585B27B9").IsUnique();

            entity.HasIndex(e => e.OrganisationId, "idx_users_organisation");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OrganisationId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("organisation_id");
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Organisation).WithMany(p => p.Users)
                .HasForeignKey(d => d.OrganisationId)
                .HasConstraintName("fk_users_organisation");
        });

        modelBuilder.Entity<Vessel>(entity =>
        {
            entity.HasKey(e => e.VesselId).HasName("PK__vessels__A352286380576E66");

            entity.ToTable("vessels");

            entity.HasIndex(e => e.ImoNumber, "UQ__vessels__1CC6B9E5EEE8CA3F").IsUnique();

            entity.HasIndex(e => e.Mmsi, "UQ__vessels__77B3115ED0F4E24D").IsUnique();

            entity.Property(e => e.VesselId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("vessel_id");
            entity.Property(e => e.CapacityTeu).HasColumnName("capacity_teu");
            entity.Property(e => e.Carrier)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("carrier");
            entity.Property(e => e.Flag)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("flag");
            entity.Property(e => e.ImoNumber)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("imo_number");
            entity.Property(e => e.Mmsi)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("mmsi");
            entity.Property(e => e.VesselName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("vessel_name");
            entity.Property(e => e.VesselType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("vessel_type");
        });

        modelBuilder.Entity<VoyageSchedule>(entity =>
        {
            entity.HasKey(e => e.VoyageId).HasName("PK__voyage_s__A2EBD414C105300D");

            entity.ToTable("voyage_schedules");

            entity.HasIndex(e => e.ServiceId, "idx_voyage_service");

            entity.HasIndex(e => e.VesselId, "idx_voyage_vessel");

            entity.Property(e => e.VoyageId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("voyage_id");
            entity.Property(e => e.PlannedArrival)
                .HasColumnType("datetime")
                .HasColumnName("planned_arrival");
            entity.Property(e => e.PlannedDeparture)
                .HasColumnType("datetime")
                .HasColumnName("planned_departure");
            entity.Property(e => e.PortSequence)
                .HasColumnType("text")
                .HasColumnName("port_sequence");
            entity.Property(e => e.ServiceId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("service_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.VesselId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("vessel_id");
            entity.Property(e => e.VoyageNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("voyage_number");

            entity.HasOne(d => d.Service).WithMany(p => p.VoyageSchedules)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_voyage_service");

            entity.HasOne(d => d.Vessel).WithMany(p => p.VoyageSchedules)
                .HasForeignKey(d => d.VesselId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_voyage_vessel");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
