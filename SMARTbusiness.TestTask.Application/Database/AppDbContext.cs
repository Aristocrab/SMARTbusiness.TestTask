using Microsoft.EntityFrameworkCore;
using SMARTbusiness.TestTask.Domain.Entities;

namespace SMARTbusiness.TestTask.Application.Database;

public class AppDbContext : DbContext
{
    public required DbSet<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; }    
    public required DbSet<ProcessEquipmentType> ProcessEquipmentTypes { get; set; }    
    public required DbSet<ProductionFacility> ProductionFacilities { get; set; }    
}