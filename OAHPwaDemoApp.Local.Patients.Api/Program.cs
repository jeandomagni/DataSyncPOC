using Hangfire;
using OAHPwaDemoApp.Local.Patients.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<IPatientsService, PatientsService>();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseIgnoredAssemblyVersionTypeResolver()
    .UseInMemoryStorage());

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

builder.Services.AddHangfireServer(options => options.Queues = new[] { "critical", "default" });

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

RecurringJob.AddOrUpdate<IPatientsService>("syncdata", patientsService => patientsService.Refresh(), "*/2 * * * *");

app.Run();
