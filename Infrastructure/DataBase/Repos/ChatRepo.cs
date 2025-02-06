using Domain.business_entities.dtos;
using Domain.business_entities.entities;
using Domain.business_logic;
using Microsoft.EntityFrameworkCore;

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

        Chat? chat = await db.Chats
            .Include(c => c.Users)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(x => x.Id == id) 
            ?? throw new Exception("not found chat");

        if (chat.Users.FirstOrDefault(x => x.Id == userId) == null)
        {
            User? user = await db.Users.FindAsync(userId) ?? throw new Exception("not found user");
            chat.Users.Add(user);
            await db.SaveChangesAsync();
        }

        List<GetMessageDTO> messages = [];
        foreach (var m in chat.Messages)
        {
            var dto = new GetMessageDTO(m.Text, m.DateTime, m.Owner.Name);
            messages.Add(dto);
        }

        List<string> names = [];
        foreach (var u in chat.Users)
            names.Add(u.Name);


        GetAndLinkChatDTO result = new(chat.Name, chat.Id, messages, names);
        return result;
    }

    public async Task<GetForListChatDTO> ReadForListAsync(Guid Id)
    {
        using SqLiteDbContext db = new();
        Chat? chat = await db.Chats.FindAsync(Id) 
            ?? throw new Exception("not found chat");

        GetForListChatDTO result = new(chat.Id, chat.Name);
        return result;
    }
}
