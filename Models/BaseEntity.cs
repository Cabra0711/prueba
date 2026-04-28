namespace PoliRiwi.Models;

// We define de main things that we want to extend from another classes to not repeat them
public abstract class BaseEntity 
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}