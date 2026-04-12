namespace JobBoardPlatform.Models;

// Base class for all database entities (OOP: Inheritance & Abstraction)
public abstract class BaseEntity
{
    public int Id { get; set; }
    
    // Automatically sets the creation date when a new record is made
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}