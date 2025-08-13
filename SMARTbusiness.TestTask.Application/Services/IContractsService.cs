using ErrorOr;
using SMARTbusiness.TestTask.Application.Dtos;

namespace SMARTbusiness.TestTask.Application.Services;

public interface IContractsService
{
    Task<ErrorOr<List<EquipmentPlacementContractDto>>> GetAllContracts();
    Task<ErrorOr<EquipmentPlacementContractDto>> CreateNewContract(CreateNewContractDto createNewContractDto);
}