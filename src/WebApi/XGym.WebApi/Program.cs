using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using XGym.Application.Common.Interfaces;
using XGym.Application.Common.Mapping;
using XGym.Application.Common.Utility;
using XGym.Application.ReservationOperations.Commands.CreateReservation;
using XGym.Persistence.Data;
using XGym.Persistence.Data.Context;
using XGym.WebApi.Extensions;
using XGym.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<XGymDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("postgreSqlConnection")));
builder.Services.AddScoped<IXGymDbContext, XGymDbContext>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters().AddValidatorsFromAssemblyContaining(typeof(CreateReservationCommandValidator));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAutoMapper(typeof(GeneralMapping).Assembly);
builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(GeneralMapping).Assembly));
builder.Services.AddSingleton<RequestResponseLoggerMiddleware>();
builder.Services.AddScoped<ITenantUtility, TenantUtility>(); 
builder.Services.ConfigureCors();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimit();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    IdentityHelper.Initialize(services).GetAwaiter().GetResult();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseIpRateLimiting();
app.UseCors("XGymCorsPolicy");
app.UseMiddleware<RequestResponseLoggerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
