﻿namespace Infrastructure;

public class CommitEntity
{
    //Commit ID
    public string ID { get; set; }
    public DateTime date { get; set; }
    //Repo ID
    public string RID { get; set; }
    //Author ID
    public string AUD { get; set; }
}