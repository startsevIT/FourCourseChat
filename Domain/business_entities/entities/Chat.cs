namespace Domain.business_entities.entities;

public class Chat(Guid id, string name, List<Message> messages, List<User> users)
{
    public Chat() : this(null, null) { }
    public Chat(string name, User user) 
        : this(Guid.NewGuid(), name, [], [user]) 
    { }

    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public List<Message> Messages { get; set; } = messages;
    public List<User> Users { get; set; } = users;
}
