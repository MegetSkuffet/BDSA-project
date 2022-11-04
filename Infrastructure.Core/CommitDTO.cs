namespace Infrastructure.Core;

public class CommitDTO{
    public record CommitDTO(string RID, DateTime date, int amountPrDay);
    public record CommitCreateDTO();

}