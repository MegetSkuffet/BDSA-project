namespace Infrastructure.Core;


public interface IAuthorService
{
    (Response Response, string Username) Create(AutherCreateDTO Author);
}