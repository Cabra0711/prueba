using PoliRiwi.Enums;

namespace PoliRiwi.Models;

public class Places : BaseEntity
{
    public string Name { get; set; }
    public SpaceType SpaceType { get; set; }
    public int Capacity { get; set; }
    public Status Status { get; set; }
    // We Use ICollection because it is a list which can allow us to read and modify is more effective than 
    // IEnumerable
    public ICollection<Reservations> Reservations { get; set; }
}