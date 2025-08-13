using SMARTbusiness.TestTask.Domain.Entities.Shared;

namespace SMARTbusiness.TestTask.Domain.Entities;

public class EquipmentPlacementContract : Entity
{
    public required ProductionFacility ProductionFacility { get; set; }
    public required ProcessEquipmentType ProcessEquipmentType { get; set; }
    public required int EquipmentUnitsCount { get; set; }
}