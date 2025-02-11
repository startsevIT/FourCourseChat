using Domain.business_entities.dtos;
using Domain.business_entities.entities;

namespace Domain.Mapping;

public static class ChatMap
{
    public static Chat Map(this CreateChatDTO dto, User user)
    {
        return new Chat
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Messages = [],
            Users = [user]
        };
    }
    public static GetAndLinkChatDTO Map(this Chat chat, List<GetMessageDTO> messages, List<string> nickNames)
    {
        return new GetAndLinkChatDTO(chat.Name, chat.Id, messages, nickNames);
    }
    public static GetForListChatDTO Map(this Chat chat)
    {
        return new GetForListChatDTO(chat.Id, chat.Name);
    }
}
