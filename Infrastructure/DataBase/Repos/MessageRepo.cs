using Domain.business_entities.dtos;
using Domain.business_entities.entities;
using Domain.business_logic;
using Domain.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repos;

public class MessageRepo : IMessageRepo
{
    public async Task<Guid> CreateAsync(CreateMessageDTO dto, Guid userId, Guid chatId)
    {
        using SqLiteDbContext db = new();

        User? user = await db.Users.FindAsync(userId) 
            ?? throw new Exception("not found user");

        Chat? chat = await db.Chats.FindAsync(chatId)
            ?? throw new Exception("not found chat");

        Message message = dto.Map(user, chat);
        await db.Messages.AddAsync(message);
        await db.SaveChangesAsync();
        return message.Id;
    }

    public async Task<GetMessageDTO> ReadAsync(Guid id)
    {
        using SqLiteDbContext db = new();

        Message? message = await db.Messages
            .Include(x => x.Owner)
            .FirstOrDefaultAsync(x => x.Id == id) 
            ?? throw new Exception("not found message");

        return message.Map(message.Owner);
    }
}
