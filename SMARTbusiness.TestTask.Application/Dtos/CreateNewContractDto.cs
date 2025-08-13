namespace SMARTbusiness.TestTask.Application.Dtos;

public class CreateNewContractDto
{
    public required string ProductionFacilityCode { get; set; }
    public required string ProcessEquipmentTypeCode { get; set; }
    public required int EquipmentUnitsCount { get; set; }
}