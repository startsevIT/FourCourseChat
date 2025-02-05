using Domain.business_entities.dtos;
namespace Domain.business_logic;

public interface IChatRepo
{
    public Task CreateAsync(CreateChatDTO dto);
    public Task<GetAndLinkChatDTO> ReadAndLinkAsync(Guid Id, Guid UserId);
    public Task<GetForListChatDTO> ReadForListAsync(Guid Id);
}
