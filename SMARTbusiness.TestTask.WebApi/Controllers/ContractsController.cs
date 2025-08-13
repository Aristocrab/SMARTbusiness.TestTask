using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SMARTbusiness.TestTask.Application.Dtos;
using SMARTbusiness.TestTask.Application.Services;

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
        return await _contractsService.GetAllContracts().ToActionResult();
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateNewContract(CreateNewContractDto createNewContractDto)
    {
        return await _contractsService.CreateNewContract(createNewContractDto).ToActionResult();
    }
}