namespace GitInsight.Core;

public record CommitDTO(string RID, string CID, string AuthorName, DateTimeOffset date);
public record CommitCreateDTO(string RID, string CID, string AuthorName, DateTimeOffset date);
