using ErrorOr;
using Hangfire;
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
    private readonly IBackgroundJobClient _backgroundJobClient;

    public ContractsService(AppDbContext dbContext, ILogger<ContractsService> logger, 
        IBackgroundJobClient backgroundJobClient)
    {
        _dbContext = dbContext;
        _logger = logger;
        _backgroundJobClient = backgroundJobClient;
    }
    
    public async Task<ErrorOr<List<EquipmentPlacementContractDto>>> GetAllContracts()
    {
        var contracts = await _dbContext
            .EquipmentPlacementContracts
            .Include(x => x.ProcessEquipmentType)
            .Include(x => x.ProductionFacility)
            .ProjectToType<EquipmentPlacementContractDto>()
            .ToListAsync();

        return contracts;
    }

    public async Task<ErrorOr<EquipmentPlacementContractDto>> CreateNewContract(CreateNewContractDto createNewContractDto)
    {
        var productionFacility = await _dbContext.ProductionFacilities
            .FirstOrDefaultAsync(x => x.Code == createNewContractDto.ProductionFacilityCode);
        if (productionFacility is null)
        {
            _logger.LogError("Production facility with Code={ProductionFacilityCode} was not found", createNewContractDto.ProductionFacilityCode);
            return Error.NotFound(description: $"Production facility with Code={createNewContractDto.ProductionFacilityCode} was not found");
        }
        
        var processEquipmentType = await _dbContext.ProcessEquipmentTypes
            .FirstOrDefaultAsync(x => x.Code == createNewContractDto.ProcessEquipmentTypeCode);
        if (processEquipmentType is null)
        {
            _logger.LogError("Process equipment type with Code={ProcessEquipmentTypeCode} was not found", createNewContractDto.ProcessEquipmentTypeCode);
            return Error.NotFound(description: $"Process equipment type with Code={createNewContractDto.ProcessEquipmentTypeCode} was not found");
        }

        var freeArea = productionFacility.StandardArea - _dbContext.EquipmentPlacementContracts
            .Where(x => x.ProductionFacility.Code == productionFacility.Code)
            .Select(x => x.ProcessEquipmentType.Area * x.EquipmentUnitsCount)
            .Sum();
        if (freeArea < processEquipmentType.Area * createNewContractDto.EquipmentUnitsCount)
        {
            _logger.LogError("Not enough free area in the production facility to place the equipment");
            return Error.Failure(description: "Not enough free area in the production facility to place the equipment");
        }

        var newContract = new EquipmentPlacementContract
        {
            ProductionFacility = productionFacility,
            ProcessEquipmentType = processEquipmentType,
            EquipmentUnitsCount = createNewContractDto.EquipmentUnitsCount
        };

        await _dbContext.EquipmentPlacementContracts.AddAsync(newContract);
        await _dbContext.SaveChangesAsync();

        _backgroundJobClient.Enqueue(() => BackgroundWork(newContract.Id));

        return newContract.Adapt<EquipmentPlacementContractDto>();
    }

    private void BackgroundWork(Guid id)
    {
        _logger.LogInformation("Background job: Contract {Id} created", id);
        Task.Delay(2000).Wait();
        _logger.LogInformation("Background job: Contract {Id} processing completed", id);
    }
}