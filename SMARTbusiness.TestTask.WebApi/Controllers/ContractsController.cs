using Microsoft.AspNetCore.Mvc;
using SMARTbusiness.TestTask.Application.Dtos;
using SMARTbusiness.TestTask.Application.Services;
using SMARTbusiness.TestTask.WebApi.Extensions;

namespace SMARTbusiness.TestTask.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IContractsService _contractsService;

    public ContractsController(IContractsService contractsService)
    {
        _contractsService = contractsService;
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<List<EquipmentPlacementContractDto>>> GetAllContracts()
    {
        var result = await _contractsService.GetAllContracts();

        return result.ToActionResult();
    }

    [HttpPost("create")]
    public async Task<ActionResult<EquipmentPlacementContractDto>> CreateNewContract(CreateNewContractDto createNewContractDto)
    {
        var result =  await _contractsService.CreateNewContract(createNewContractDto);
        
        return result.ToActionResult();
    }
}