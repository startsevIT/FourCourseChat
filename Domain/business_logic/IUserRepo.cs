using Domain.business_entities.dtos;
namespace Domain.business_logic;

public interface IUserRepo
{
    public void Register(RegisterUserDTO dto);
    public string Login(LoginUserDTO dto);
    public GetAccountUserDTO Read(Guid id);
}
