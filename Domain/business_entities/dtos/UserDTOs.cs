namespace Domain.business_entities.dtos;

public class RegisterUserDTO(
    string Name,
    string Login,
    string Password);
public class LoginUserDTO(
    string Login,
    string Password);
public class GetAccountUserDTO(
    string Name,
    List<GetForListChatDTO> Chats);