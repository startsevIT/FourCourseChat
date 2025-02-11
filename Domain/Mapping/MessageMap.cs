using Domain.business_entities.dtos;
using Domain.business_entities.entities;

namespace Domain.Mapping;

public static class MessageMap
{
    public static Message Map(this CreateMessageDTO dto, User user, Chat chat)
    {
        return new Message
        {
            Id = Guid.NewGuid(),
            Text = dto.Text,
            DateTime = DateTime.Now,
            Owner = user,
            Chat = chat
        };
    }
    public static GetMessageDTO Map(this Message message, User user)
    {
        return new GetMessageDTO(message.Text, message.DateTime, user.Name);
    }
}
