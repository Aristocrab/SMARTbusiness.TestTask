using SMARTbusiness.TestTask.Domain.Entities;
using SMARTbusiness.TestTask.Domain.ValueObjects;

namespace SMARTbusiness.TestTask.Application.Database;

public class DbSeeder
{
    private readonly AppDbContext _dbContext;

    public DbSeeder(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SeedDb()
    {
        if (!_dbContext.ProductionFacilities.Any())
        {
            var facilities = new List<ProductionFacility>
            {
                new()
                {
                    Code = ProductionFacilityCode.From("FAC001"),
                    Name = "Main Plant",
                    StandardArea = 5000m
                },
                new()
                {
                    Code = ProductionFacilityCode.From("FAC002"),
                    Name = "Secondary Plant",
                    StandardArea = 3000m
                },
                new()
                {
                    Code = ProductionFacilityCode.From("FAC003"),
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
                    Code = ProcessEquipmentTypeCode.From("EQ001"),
                    Name = "Conveyor Belt",
                    Area = 50m
                },
                new()
                {
                    Code = ProcessEquipmentTypeCode.From("EQ002"),
                    Name = "Hydraulic Press",
                    Area = 80m
                },
                new()
                {
                    Code = ProcessEquipmentTypeCode.From("EQ003"),
                    Name = "CNC Machine",
                    Area = 120m
                },
                new()
                {
                    Code = ProcessEquipmentTypeCode.From("EQ004"),
                    Name = "Welding Station",
                    Area = 30m
                },
                new()
                {
                    Code = ProcessEquipmentTypeCode.From("EQ005"),
                    Name = "Assembly Robot",
                    Area = 45m
                }
            };

            _dbContext.ProcessEquipmentTypes.AddRange(equipmentTypes);
        }

        await _dbContext.SaveChangesAsync();
    }
}