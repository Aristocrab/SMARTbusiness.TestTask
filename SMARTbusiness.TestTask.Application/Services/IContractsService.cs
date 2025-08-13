using FluentResults;
using SMARTbusiness.TestTask.Application.Dtos;

namespace SMARTbusiness.TestTask.Application.Services;

public interface IContractsService
{
    Task<Result<List<EquipmentPlacementContractDto>>> GetAllContracts();
    Task<Result> CreateNewContract(CreateNewContractDto createNewContractDto);
}