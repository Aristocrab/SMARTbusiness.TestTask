using System.Diagnostics.Contracts;
using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SMARTbusiness.TestTask.Application.Database;
using SMARTbusiness.TestTask.Application.Dtos;
using SMARTbusiness.TestTask.Domain.Entities;

namespace SMARTbusiness.TestTask.Application.Services;

public class ContractsService : IContractsService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ContractsService> _logger;

    public ContractsService(AppDbContext dbContext, ILogger<ContractsService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<Result<List<EquipmentPlacementContractDto>>> GetAllContracts()
    {
        var contracts = await _dbContext
            .EquipmentPlacementContracts
            .Include(x => x.ProcessEquipmentType)
            .Include(x => x.ProductionFacility)
            .ProjectToType<EquipmentPlacementContractDto>()
            .ToListAsync();

        return Result.Ok(contracts);
    }

    public async Task<Result> CreateNewContract(CreateNewContractDto createNewContractDto)
    {
        var productionFacility = await _dbContext.ProductionFacilities
            .FirstOrDefaultAsync(x => x.Code == createNewContractDto.ProductionFacilityCode);
        if (productionFacility is null)
        {
            _logger.LogError("Production facility with Code={ProductionFacilityCode} was not found", createNewContractDto.ProductionFacilityCode);
            return Result.Fail($"Production facility with Code={createNewContractDto.ProductionFacilityCode} was not found");
        }
        
        var processEquipmentType = await _dbContext.ProcessEquipmentTypes
            .FirstOrDefaultAsync(x => x.Code == createNewContractDto.ProcessEquipmentTypeCode);
        if (processEquipmentType is null)
        {
            _logger.LogError("Process equipment type with Code={ProcessEquipmentTypeCode} was not found", createNewContractDto.ProcessEquipmentTypeCode);
            return Result.Fail($"Process equipment type with Code={createNewContractDto.ProcessEquipmentTypeCode} was not found");
        }

        var freeArea = productionFacility.StandardArea - _dbContext.EquipmentPlacementContracts
            .Where(x => x.ProductionFacility.Code == productionFacility.Code)
            .Select(x => x.ProcessEquipmentType.Area * x.EquipmentUnitsCount)
            .Sum();
        if (freeArea < processEquipmentType.Area * createNewContractDto.EquipmentUnitsCount)
        {
            _logger.LogError("Not enough free area in the production facility to place the equipment");
            return Result.Fail("Not enough free area in the production facility to place the equipment");
        }

        var newContract = new EquipmentPlacementContract
        {
            ProductionFacility = productionFacility,
            ProcessEquipmentType = processEquipmentType,
            EquipmentUnitsCount = createNewContractDto.EquipmentUnitsCount
        };

        await _dbContext.EquipmentPlacementContracts.AddAsync(newContract);
        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}