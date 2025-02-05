using Domain.business_entities.dtos;

namespace Domain.business_logic;

public interface IMessageRepo
{
    public Task CreateAsync(CreateMessageDTO dto);
    public Task<GetMessageDTO> ReadAsync(Guid id);
}
