namespace Domain.business_entities.entities;

public class Chat
{
    public Guid Id { get; set; } 
    public string Name { get; set; } 
    public List<Message> Messages { get; set; }
    public List<User> Users { get; set; } 
}
