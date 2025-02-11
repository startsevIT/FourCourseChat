using Domain.business_entities.dtos;
using Domain.business_entities.entities;
using Domain.business_logic;
using Domain.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repos;

public class ChatRepo : IChatRepo
{
    public async Task CreateAsync(CreateChatDTO dto, Guid userId)
    {
        using SqLiteDbContext db = new ();
        User? user = await db.Users.FindAsync(userId) 
            ?? throw new Exception("not found user");

        Chat chat = dto.Map(user);
        db.Chats.Add(chat);
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
            User? user = await db.Users.FindAsync(userId) 
                ?? throw new Exception("not found user");
            chat.Users.Add(user);
            await db.SaveChangesAsync();
        }

        List<GetMessageDTO> messages = [];
        foreach (var m in chat.Messages)
        {
            var dto = m.Map(m.Owner);
            messages.Add(dto);
        }

        //Это
        List<string> names = [..chat.Users.Select(x => x.Name)];


        //И это - одно и то же
        //List<string> names = [];
        //foreach (var u in chat.Users)
        //    names.Add(u.Name);


       return chat.Map(messages, names);
    }

    public async Task<GetForListChatDTO> ReadForListAsync(Guid Id)
    {
        using SqLiteDbContext db = new();
        Chat? chat = await db.Chats.FindAsync(Id) 
            ?? throw new Exception("not found chat");

        return chat.Map();
    }
}
