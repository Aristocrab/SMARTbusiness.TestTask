using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using SMARTbusiness.TestTask.Application.Database;
using SMARTbusiness.TestTask.Application.Dtos;
using SMARTbusiness.TestTask.Application.Services;
using SMARTbusiness.TestTask.Domain.Entities;

namespace SMARTbusiness.TestTask.Tests;

public class ContractsServiceTests
{
    private readonly ContractsService _contractsService;

    public ContractsServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("ContractsServiceTests")
            .Options;

        var dbContext = new AppDbContext(options);

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        
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
            }
        };

        dbContext.ProductionFacilities.AddRange(facilities);

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
            }
        };
        
        dbContext.ProcessEquipmentTypes.AddRange(equipmentTypes);

        var contracts = new List<EquipmentPlacementContract>
        {
            new()
            {
                ProductionFacility = facilities[0],
                ProcessEquipmentType = equipmentTypes[0],
                EquipmentUnitsCount = 1
            },
            
            new()
            {
                ProductionFacility = facilities[1],
                ProcessEquipmentType = equipmentTypes[1],
                EquipmentUnitsCount = 1
            }
        };
        
        dbContext.EquipmentPlacementContracts.AddRange(contracts);

        dbContext.SaveChanges();

        _contractsService = new ContractsService(dbContext, Substitute.For<ILogger<ContractsService>>());
    }

    [Fact]
    public async Task GetAllContracts_ShouldReturnAllContracts()
    {
        // Act
        var contracts = await _contractsService.GetAllContracts();
        
        // Assert
        contracts.IsSuccess.ShouldBeTrue();
        contracts.Value.ShouldNotBeEmpty();
    }
    
    [Fact]
    public async Task CreateNewContract_ShouldCreateContract()
    {
        // Arrange
        var newContractDto = new CreateNewContractDto
        {
            ProductionFacilityCode = "FAC001",
            ProcessEquipmentTypeCode = "EQ002",
            EquipmentUnitsCount = 1
        };
        
        // Act 
        var result = await _contractsService.CreateNewContract(newContractDto);
        
        // Assert
        result.IsSuccess.ShouldBeTrue();
    }
    
    [Fact]
    public async Task CreateNewContract_ShouldReturnError_IfProductionFacilityDoesNotExist()
    {
        // Arrange
        var newContractDto = new CreateNewContractDto
        {
            ProductionFacilityCode = "TEST000",
            ProcessEquipmentTypeCode = "EQ002",
            EquipmentUnitsCount = 1
        };
        
        // Act 
        var result = await _contractsService.CreateNewContract(newContractDto);
        
        // Assert
        result.IsSuccess.ShouldBeFalse();
    }
    
    [Fact]
    public async Task CreateNewContract_ShouldReturnError_IfProcessEquipmentTypeDoesNotExist()
    {
        // Arrange
        var newContractDto = new CreateNewContractDto
        {
            ProductionFacilityCode = "FAC001",
            ProcessEquipmentTypeCode = "TEST000",
            EquipmentUnitsCount = 1
        };
        
        // Act 
        var result = await _contractsService.CreateNewContract(newContractDto);
        
        // Assert
        result.IsSuccess.ShouldBeFalse();
    }
    
    [Fact]
    public async Task CreateNewContract_ShouldReturnError_IfNoAreaForEquipment()
    {
        // Arrange
        var newContractDto = new CreateNewContractDto
        {
            ProductionFacilityCode = "FAC002",
            ProcessEquipmentTypeCode = "EQ002",
            EquipmentUnitsCount = 100
        };
        
        // Act 
        var result = await _contractsService.CreateNewContract(newContractDto);
        
        // Assert
        result.IsSuccess.ShouldBeFalse();
    }
}