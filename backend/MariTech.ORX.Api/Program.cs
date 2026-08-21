using Microsoft.EntityFrameworkCore;
using MariTech.ORX.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// SERVICES
// ============================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================================
// DATABASE
// ============================================

builder.Services.AddDbContext<ORXDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ============================================
// CORS
// ============================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ============================================
// APPLICATION
// ============================================

var app = builder.Build();

// ============================================
// MIDDLEWARE
// ============================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

// Ensure the small V1 workflow extension table exists alongside the existing database schema.
// This keeps the existing scaffolded MARITECH_ORX schema intact while persisting fields
// that are not represented on dbo.shipments/cargo (mode, container count, services, etc.).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ORXDbContext>();
    await db.Database.ExecuteSqlRawAsync(@"
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
END");
}

app.Run();