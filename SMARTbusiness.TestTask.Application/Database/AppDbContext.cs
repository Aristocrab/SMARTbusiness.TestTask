using Microsoft.EntityFrameworkCore;
using SMARTbusiness.TestTask.Domain.Entities;

namespace SMARTbusiness.TestTask.Application.Database;

public class AppDbContext : DbContext
{
    public DbSet<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }    
    public DbSet<ProcessEquipmentType> ProcessEquipmentTypes { get; set; }    
    public DbSet<ProductionFacility> ProductionFacilities { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}