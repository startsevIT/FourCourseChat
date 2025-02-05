using Domain.business_entities.dtos;
namespace Domain.business_logic;

public interface IUserRepo
{
    public Task RegisterAsync(RegisterUserDTO dto);
    public Task<string> LoginAsync(LoginUserDTO dto);
    public Task<GetAccountUserDTO> ReadAsync(Guid id);
}
