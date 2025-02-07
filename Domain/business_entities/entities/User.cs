namespace Domain.business_entities.entities;

public class User(Guid id, string name, string login, string password, List<Chat> chats)
{
    public User() : this(null, null, null) { }
    public User(string name, string login, string password) 
        : this(Guid.NewGuid(), name, login, password, []) { }

    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Login { get; set; } = login;
    public string Password { get; set; } = password;
    public List<Chat> Chats { get; set; } = chats;
}
