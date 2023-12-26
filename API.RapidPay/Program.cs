using API.RapidPay.Swagger;
using Core.RapidPay.Automapper.Profiles.UserProfiles;
using Core.RapidPay.Handlers;
using Core.RapidPay.Options;
using Core.RapidPay.Services.CreditCards;
using Core.RapidPay.Services.Identity;
using Core.RapidPay.Services.UFE;
using DAL.RapidPay.Context;
using Interfaces.RapidPay.CreditCards;
using Interfaces.RapidPay.Identity;
using Interfaces.RapidPay.UFE;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Shared.RapidPay.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddNewtonsoftJson(o => { o.SerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy(), }); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtOptions = new JwtOptions();
builder.Configuration.GetSection(JwtOptions.Key).Bind(jwtOptions);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Key));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<RapidPayContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("Migrations.RapidPay")));

builder.Services.AddAutoMapper(typeof(UserMapperProfile).Assembly);

builder.Services.AddSingleton<IUFEService, UFEService>();

builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddTransient<ICreditCardCreationService, CreditCardCreationService>();
builder.Services.AddTransient<ICreditCardValidationService, CreditCardValidationService>();
builder.Services.AddTransient<ICreditCardService, CreditCardService>();
//builder.Services.AddTransient<IPaymentHandler, PaymentHandlerBase>();
builder.Services.Chain<IPaymentHandler>()
    .Add<CreditCardOwnerValidatorHandler>()
    .Add<CreditCardNumberValidatorHandler>()
    .Add<CreditCardExpiryDateValidatorHandler>()
    .Add<CreditCardCVCValidatorHandler>()
    .Add<CreditCardPaymentHandler>()
    .Configure();

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

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();