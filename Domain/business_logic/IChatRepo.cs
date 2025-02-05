using Domain.business_entities.dtos;
namespace Domain.business_logic;

internal interface IChatRepo
{
    public void Create(CreateChatDTO dto);
    public GetAndLinkChatDTO ReadAndLink(Guid Id, Guid UserId);
    public GetForListChatDTO ReadForList(Guid Id);
}
