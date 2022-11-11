namespace GitInsight.Core;

public record RepositoryDTO(string ID, string lastCommitSha);

public record RepositoryCreateDTO(string ID, string lastCommitSha);

public record RepositoryUpdateDTO(string ID, string lastCommitSha);

