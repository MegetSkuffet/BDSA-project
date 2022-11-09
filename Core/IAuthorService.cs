namespace GitInsight.Core;


public interface IAuthorService
{
    (Response Response, string Username) Create(AuthorCreateDTO Author);
    IReadOnlyCollection<AuthorDTO> ReadAllByAuthor();
}
