namespace Infrastructure;

public class CommitEntity
{
    //Commit ID
    public string RID { get; set; }
    
    public string CID { get; set; }
    
    public string Author { get; set; }
    public DateTimeOffset date { get; set; }
}