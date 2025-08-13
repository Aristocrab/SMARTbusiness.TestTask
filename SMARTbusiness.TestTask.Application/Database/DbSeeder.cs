using Microsoft.Extensions.Logging;
using SMARTbusiness.TestTask.Domain.Entities;

namespace SMARTbusiness.TestTask.Application.Database;

public class DbSeeder
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DbSeeder> _logger;

    public DbSeeder(AppDbContext dbContext, ILogger<DbSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedDb()
    {
        if (!_dbContext.ProductionFacilities.Any())
        {
            var facilities = new List<ProductionFacility>
            {
                new()
                {
                    Code = "FAC001",
                    Name = "Main Plant",
                    StandardArea = 5000m
                },
                new()
                {
                    Code = "FAC002",
                    Name = "Secondary Plant",
                    StandardArea = 3000m
                },
                new()
                {
                    Code = "FAC003",
                    Name = "Test Facility",
                    StandardArea = 1500m
                }
            };

            _dbContext.ProductionFacilities.AddRange(facilities);
        }

        if (!_dbContext.ProcessEquipmentTypes.Any())
        {
            var equipmentTypes = new List<ProcessEquipmentType>
            {
                new()
                {
                    Code = "EQ001",
                    Name = "Conveyor Belt",
                    Area = 50m
                },
                new()
                {
                    Code = "EQ002",
                    Name = "Hydraulic Press",
                    Area = 80m
                },
                new()
                {
                    Code = "EQ003",
                    Name = "CNC Machine",
                    Area = 120m
                },
                new()
                {
                    Code = "EQ004",
                    Name = "Welding Station",
                    Area = 30m
                },
                new()
                {
                    Code = "EQ005",
                    Name = "Assembly Robot",
                    Area = 45m
                }
            };

            _dbContext.ProcessEquipmentTypes.AddRange(equipmentTypes);
        }
        
        if (!_dbContext.EquipmentPlacementContracts.Any())
        {
            var facility1 = _dbContext.ProductionFacilities.First(x => x.Name == "Main Plant");
            var facility2 = _dbContext.ProductionFacilities.First(x => x.Name == "Secondary Plant");

            var equipmentType1 = _dbContext.ProcessEquipmentTypes.First(x => x.Name == "Conveyor Belt");
            var equipmentType2 = _dbContext.ProcessEquipmentTypes.First(x => x.Name == "Hydraulic Press");

            var contract1 = new EquipmentPlacementContract
            {
                ProductionFacility = facility1,
                ProcessEquipmentType = equipmentType1,
                EquipmentUnitsCount = 5
            };

            var contract2 = new EquipmentPlacementContract
            {
                ProductionFacility = facility2,
                ProcessEquipmentType = equipmentType2,
                EquipmentUnitsCount = 10
            };

            _dbContext.EquipmentPlacementContracts.AddRange(contract1, contract2);
        }
        
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Db seeded successfully");
    }
}