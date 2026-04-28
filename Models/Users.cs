namespace PoliRiwi.Models;

// We define a BaseEntity and we extend this for avoiding rendurance
// in the system and do not repeating the same every time we define this for each Model

public class Users : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Document {get; set;}
    public string Phone { get; set; }
    // We Use ICollection because it is a list which can allow us to read and modify is more effective than 
    // IEnumerable
    public ICollection<Reservations> Reservations { get; set; }
}