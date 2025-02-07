namespace Domain.business_entities.dtos;

public record class CreateChatDTO(
    string Name);
public record class GetAndLinkChatDTO(
    string Name, 
    Guid Id, 
    List<GetMessageDTO> Messages,
    List<string> UserNames);
public record class GetForListChatDTO(
    Guid Id, 
    string Name);
