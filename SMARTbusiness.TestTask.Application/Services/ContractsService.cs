using FluentResults;
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
    
    public Task<Result<List<EquipmentPlacementContract>>> GetAllContracts()
    {
        throw new NotImplementedException();
    }

    public Task<Result> CreateNewContract(CreateNewContractDto createNewContractDto)
    {
        throw new NotImplementedException();
    }
}