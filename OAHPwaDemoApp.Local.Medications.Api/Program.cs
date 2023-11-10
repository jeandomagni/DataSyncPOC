using Hangfire;
using Hangfire.Storage.SQLite;
using OAHPwaDemoApp.Local.Medications.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<IMedicationsService, MedicationsService>();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseIgnoredAssemblyVersionTypeResolver()
    .UseInMemoryStorage());
builder.Services.AddHangfireServer(options => options.Queues = new[] { "critical", "default" });

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader()
                                                  .AllowAnyMethod(); ;
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHangfireDashboard(string.Empty);

RecurringJob.AddOrUpdate<IMedicationsService>("syncdata", medicationsService => medicationsService.Refresh(), "*/2 * * * *");


app.Run();
