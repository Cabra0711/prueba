using PoliRiwi.Enums;

namespace PoliRiwi.Models;

public class Reservations : BaseEntity
{
    public int UserId { get; set; }
    public Users User { get; set; }
    public int PlaceId { get; set; }
    public Places Place { get; set; }
    public Status Status { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}