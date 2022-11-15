using System.Text.Json.Serialization;

namespace GitInsight.Core;

public record CommitDTO([property: JsonIgnore]string RID, string CID, string AuthorName, DateTimeOffset date);
public record CommitCreateDTO(string RID, string CID, string AuthorName, DateTimeOffset date);
