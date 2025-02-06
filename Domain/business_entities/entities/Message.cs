namespace Domain.business_entities.entities;

public class Message(Guid id, string text, DateTime dateTime, User owner, Chat chat)
{
    public Message(string text, User owner, Chat chat) 
        : this(Guid.NewGuid(),text,DateTime.Now,owner,chat) 
    { }

    public Guid Id { get; set; } = id;
    public string Text { get; set; } = text;
    public DateTime DateTime { get; set; } = dateTime;
    public User Owner { get; set; } = owner;
    public Chat Chat { get; set; } = chat;
}
