using SMARTbusiness.TestTask.Domain.Entities.Shared;
using SMARTbusiness.TestTask.Domain.ValueObjects;

namespace SMARTbusiness.TestTask.Domain.Entities;

public class ProductionFacility : Entity
{
    public required ProductionFacilityCode Code { get; set; }
    public required string Name { get; set; }
    public required decimal StandardArea { get; set; }

    public ICollection<EquipmentPlacementContract> EquipmentPlacementContracts { get; set; } = [];
}