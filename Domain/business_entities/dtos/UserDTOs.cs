namespace Domain.business_entities.dtos;

public record class RegisterUserDTO(
    string Name,
    string Login,
    string Password);
public record class LoginUserDTO(
    string Login,
    string Password);
public record class GetAccountUserDTO(
    string Name,
    List<GetForListChatDTO> Chats);