using Domain.business_entities.dtos;
using Domain.business_entities.entities;
using Domain.business_logic;

namespace Infrastructure.DataBase.Repos;

public class MessageRepo : IMessageRepo
{
    public async Task CreateAsync(CreateMessageDTO dto)
    {
        using SqLiteDbContext db = new();
        User? user = await db.Users.FindAsync(dto.UserId) 
            ?? throw new Exception("not found user");

        Chat? chat = await db.Chats.FindAsync(dto.ChatId)
            ?? throw new Exception("not found chat");

        Message message = new(dto.Text, user, chat);

        await db.Messages.AddAsync(message);
        await db.SaveChangesAsync();
    }

    public async Task<GetMessageDTO> ReadAsync(Guid id)
    {
        using SqLiteDbContext db = new();
        Message? message = await db.Messages.FindAsync(id) 
            ?? throw new Exception("not found message");

        GetMessageDTO result = new(message.Text, message.DateTime, message.Owner.Name);
        return result;
    }
}
