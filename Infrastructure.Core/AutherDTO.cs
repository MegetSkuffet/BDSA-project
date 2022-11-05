namespace Infrastructure.Core;

public class AuthorDTO{

    public record AuthorDTO(string Username, string RID, DateTime date, int amountPerDay);

    public AuthorCreateDTO(string Username, string RID, DateTime date, int amountPerDay);

    
}