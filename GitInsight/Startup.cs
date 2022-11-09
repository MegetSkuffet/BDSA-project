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
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        //services.AddDbContext<GitInsightContext>(o => o.UseSqlite(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddControllers();       
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseEndpoints(b =>
        {
            b.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
        });

        using var scope = app.ApplicationServices.CreateScope();

        //var dbContext = scope.ServiceProvider.GetRequiredService<GitInsightContext>();
        //dbContext.Database.Migrate();
    }
}