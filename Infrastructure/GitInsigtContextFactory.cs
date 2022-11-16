﻿using Microsoft.EntityFrameworkCore;


namespace Infrastructure;
using Microsoft.EntityFrameworkCore.Design;
public class GitInsigtContextFactory : IDesignTimeDbContextFactory<GitInsightContext>
{
    public GitInsightContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<GitInsightContext>();
        optionBuilder.UseSqlite("Data source=GitInsight.db");

        return new GitInsightContext(optionBuilder.Options);
    }
}