using Domain.business_entities.dtos;
using Domain.business_entities.entities;
using Domain.business_logic;

namespace Infrastructure.DataBase.Repos;

public class ChatRepo : IChatRepo
{
    public async Task CreateAsync(CreateChatDTO dto)
    {
        using SqLiteDbContext db = new ();
        User? user = await db.Users.FindAsync(dto.UserId) 
            ?? throw new Exception("not found user");

        Chat chat = new(dto.Name, user);
        await db.Chats.AddAsync(chat);
        await db.SaveChangesAsync();
    }

    public async Task<GetAndLinkChatDTO> ReadAndLinkAsync(Guid id, Guid userId)
    {
        using SqLiteDbContext db = new();
        Chat? chat = await db.Chats.FindAsync(id) 
            ?? throw new Exception("not found chat");

        User? user = await db.Users.FindAsync(userId) 
            ?? throw new Exception("not found user");

        if (!user.Chats.All(x => x.Id == chat.Id))
        {
            chat.Users.Add(user);
            await db.SaveChangesAsync();
        }

        GetAndLinkChatDTO result = new(chat.Name,chat.Id,)
    }

    public Task<GetForListChatDTO> ReadForListAsync(Guid Id)
    {
        throw new NotImplementedException();
    }
}
