using Domain.business_entities.dtos;

namespace Domain.business_logic;

public interface IMessageRepo
{
    public void Create(CreateMessageDTO dto);
    public GetMessageDTO Read(Guid Id);
}
