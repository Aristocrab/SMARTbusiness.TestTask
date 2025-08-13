using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SMARTbusiness.TestTask.Application.Database;
using SMARTbusiness.TestTask.Application.Dtos;
using SMARTbusiness.TestTask.Domain.Entities;

namespace SMARTbusiness.TestTask.Application.Services;

public class ContractsService : IContractsService
{
    private readonly AppDbContext _dbContext;

    public ContractsService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<List<EquipmentPlacementContractDto>>> GetAllContracts()
    {
        var contracts = await _dbContext
            .EquipmentPlacementContracts
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
            return Result.Fail($"Production facility with Code={createNewContractDto.ProductionFacilityCode} was not found");
        }
        
        var processEquipmentType = await _dbContext.ProcessEquipmentTypes
            .FirstOrDefaultAsync(x => x.Code == createNewContractDto.ProcessEquipmentTypeCode);
        if (processEquipmentType is null)
        {
            return Result.Fail($"Process equipment type with Code={createNewContractDto.ProcessEquipmentTypeCode} was not found");
        }

        var freeArea = productionFacility.StandardArea - _dbContext.EquipmentPlacementContracts
            .Where(x => x.ProductionFacility.Code == productionFacility.Code)
            .Select(x => x.ProcessEquipmentType.Area)
            .Sum();
        if (freeArea < processEquipmentType.Area)
        {
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