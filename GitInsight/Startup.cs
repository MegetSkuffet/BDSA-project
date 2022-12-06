﻿using GitInsight.Core.Factories;
using GitInsight.Core.Services;
using GitInsight.Factories;
using Infrastructure.Services;

namespace GitInsight;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        
        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:5120", "https://localhost:5120");
                }
            ));
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //services.AddDbContext<GitInsightContext>(o => o.UseSqlite(connectionString));
        services.AddDbContext<GitInsightContext>(o => o.UseSqlite("Data Source=GitInsight.db"));
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddScoped<ICloneService, CloneService>();
        services.AddScoped<IDatabase, Database>();
        services.AddScoped<IHostedService, ApplicationLifetimeService>();
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddTransient<ClonedRepositoryFactory.ClonedRepository>();
        services.AddTransient<IClonedRepositoryFactory, ClonedRepositoryFactory>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        // app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();

        app.UseEndpoints(b =>
        {
            b.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
        });

        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<GitInsightContext>();
        dbContext.Database.Migrate();
    }
}
