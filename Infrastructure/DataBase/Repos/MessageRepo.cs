using Domain.business_entities.dtos;
using Domain.business_entities.entities;
using Domain.business_logic;
using Domain.Mapping;
using Microsoft.EntityFrameworkCore;

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

        await db.Messages.AddAsync(dto.Map(user, chat));
        await db.SaveChangesAsync();
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
