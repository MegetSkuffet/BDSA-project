namespace GitInsight.Core;

public record RepositoryDto(string Id, string LastCommitSha);

public record RepositoryCreateDto(string Id, string LastCommitSha);

public record RepositoryUpdateDto(string Id, string LastCommitSha);

