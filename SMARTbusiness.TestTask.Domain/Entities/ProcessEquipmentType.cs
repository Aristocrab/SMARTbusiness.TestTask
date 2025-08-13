using SMARTbusiness.TestTask.Domain.Entities.Shared;

namespace SMARTbusiness.TestTask.Domain.Entities;

public class ProcessEquipmentType : Entity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required decimal Area { get; set; }
}