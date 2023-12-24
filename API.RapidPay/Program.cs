using Core.RapidPay.Automapper;
using DAL.RapidPay.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Shared.RapidPay.Options;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppOptions>(builder.Configuration.GetSection(AppOptions.Key));

builder.Services.AddDbContext<RapidPayContext>(
    options =>
    options.UseSqlServer(
            Configuration.GetConnectionString("Default"),
            x => x.MigrationsAssembly("Migrations.RapidPay")));

builder.Services.AddControllers().AddNewtonsoftJson(o => { o.SerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy(), }); });
builder.Services.AddAutoMapper(typeof(UserMapperProfile).Assembly);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
using (var context = scope.ServiceProvider.GetService<RapidPayContext>())
    context.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.Run();