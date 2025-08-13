using SMARTbusiness.TestTask.Domain.ValueObjects;

namespace SMARTbusiness.TestTask.Application.Dtos;

public class CreateNewContractDto
{
    public required ProductionFacilityCode ProductionFacilityCode { get; set; }
    public required ProcessEquipmentTypeCode ProcessEquipmentTypeCode { get; set; }
    public required int EquipmentUnitsCount { get; set; }
}