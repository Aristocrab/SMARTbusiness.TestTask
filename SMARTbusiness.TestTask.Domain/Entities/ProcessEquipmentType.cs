using SMARTbusiness.TestTask.Domain.Entities.Shared;
using SMARTbusiness.TestTask.Domain.ValueObjects;

namespace SMARTbusiness.TestTask.Domain.Entities;

public class ProcessEquipmentType : Entity
{
    public required Code Code { get; set; }
    public required string Name { get; set; }
    public required decimal Area { get; set; }
}