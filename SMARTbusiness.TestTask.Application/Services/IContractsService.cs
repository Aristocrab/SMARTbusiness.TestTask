using FluentResults;
using SMARTbusiness.TestTask.Application.Dtos;
using SMARTbusiness.TestTask.Domain.Entities;

namespace SMARTbusiness.TestTask.Application.Services;

public interface IContractsService
{
    Task<Result<List<EquipmentPlacementContract>>> GetAllContracts();
    Task<Result> CreateNewContract(CreateNewContractDto createNewContractDto);
}