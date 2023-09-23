using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Core.DataAccess;
using Core.DataAccess.MongoOptions;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Business.DependencyResolvers;
using Business.ThirdPartyServices.StorageServices.Local;
using Business.ThirdPartyServices.StorageServices.Azure;
using Business.ThirdPartyServices.StorageServices;
using Business.ThirdPartyServices.MessageBrokerServices;
using Business.ThirdPartyServices.MessageBrokerServices.RabbitMQ;
using Entities.Dtos;
using Business.ThirdPartyServices.PaymentServices.PayPal;
using Business.ThirdPartyServices.MessageBrokerServices.NewFolder;
using Business.ThirdPartyServices.PaymentServices.IyziPay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200", "http://localhost:59814")
               .AllowAnyHeader();
    });
});
builder.Services.AddOptions();
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });
builder.Services.AddMemoryCache();



builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddDependencyResolvers(new ICoreModule[] {
    new CoreModule()
});


//builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));

builder.Services.Load();
builder.Services.AddStorage<AzureStorageAdapter>();
builder.Services.AddPaymentService<IyzipayAdapter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
