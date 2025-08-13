namespace SMARTbusiness.TestTask.Application.Dtos;

public class EquipmentPlacementContractDto
{
    public required string ProductionFacilityName { get; set; }
    public required string ProcessEquipmentTypeName { get; set; }
    public required int EquipmentUnitsCount { get; set; }
}