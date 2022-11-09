namespace GitInsight.Core;

public record AuthorDTO(string Username, string RID, DateTime date, int amountPerDay);

public record AuthorCreateDTO(string Username, string RID, DateTime date, int amountPerDay);
