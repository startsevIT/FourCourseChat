using Domain.business_entities.dtos;
using Domain.business_entities.entities;

namespace Domain.Mapping;

public static class UserMap
{
    public static User Map(this RegisterUserDTO dto)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Login = dto.Login,
            Password = dto.Password,
            Chats = []
        };
    }
    public static GetAccountUserDTO Map(this User user, List<GetForListChatDTO> chats)
    {
        return new GetAccountUserDTO(user.Name, chats);
    }
}
