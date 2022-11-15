namespace GitInsight.Core;

public record RepositoryDTO(string ID, string LastcommitSha);

public record RepositoryCreateDTO(string ID, string LastcommitSha);

public record RepositoryUpdateDTO(string ID, string LastcommitSha);

