using System.Text.Json.Serialization;

namespace GitInsight.Core;

public record CommitDTO([property: JsonIgnore]string RID, DateTime date, int amountPrDay);
public record CommitCreateDTO(string RID, DateTime date, int amountPrDay);
