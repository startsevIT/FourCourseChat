using Domain.business_entities.dtos;

namespace Domain.business_logic;

public interface IMessageRepo
{
    public Task<Guid> CreateAsync(CreateMessageDTO dto, Guid userId, Guid chatId);
    public Task<GetMessageDTO> ReadAsync(Guid id);
}
