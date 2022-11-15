namespace GitInsight.Core.Services;

public interface ICloneService
{
    //Returns local path of repository after cloning
    Task<string> CloneRepositoryFromWebAsync(string gitUser, string gitRepository);
    
    //Finds path of repository on machine
    bool FindRepositoryOnMachine(string gitRepository, out string repoPath);
}