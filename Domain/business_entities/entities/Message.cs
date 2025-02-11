namespace Domain.business_entities.entities;

public class Message
{
    public Guid Id { get; set; } 
    public string Text { get; set; } 
    public DateTime DateTime { get; set; } 
    public User Owner { get; set; } 
    public Chat Chat { get; set; } 
}
