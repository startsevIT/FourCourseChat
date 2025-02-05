namespace Domain.business_entities.dtos;

public record class CreateMessageDTO(
    string Text, 
    Guid UserId, 
    Guid ChatId);
public record class GetMessageDTO(
    string Text, 
    DateTime DateTime, 
    string UserName);
