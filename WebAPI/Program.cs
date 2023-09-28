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
using Business.ThirdPartyServices.StorageServices.Azure;
using Business.ThirdPartyServices.StorageServices;
using Business.ThirdPartyServices.MessageBrokerServices;
using Business.ThirdPartyServices.MessageBrokerServices.RabbitMQ;
using Entities.Dtos;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System.Reflection;
using Business.ThirdPartyServices.PaymentServices.IyziPay;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Microsoft.AspNetCore.HttpLogging;
using Serilog.Context;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Collections.ObjectModel;
using WebAPI.Configurations;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 100000000; // 100 MB
});
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
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),

            NameClaimType = ClaimTypes.Name
        };
    });



builder.Services.AddDependencyResolvers(new ICoreModule[] {
    new CoreModule()
});
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacBusinessModule());
});


SqlColumn sqlColumn = new SqlColumn();
sqlColumn.ColumnName = "UserName";
sqlColumn.DataType = System.Data.SqlDbType.NVarChar;
sqlColumn.PropertyName = "UserName";
sqlColumn.DataLength = 50;
sqlColumn.AllowNull = true;
ColumnOptions columnOpt = new ColumnOptions();
columnOpt.Store.Remove(StandardColumn.Properties);
columnOpt.Store.Add(StandardColumn.LogEvent);
columnOpt.AdditionalColumns = new Collection<SqlColumn> { sqlColumn };

Logger log = new LoggerConfiguration()
    //.WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer(
    connectionString: builder.Configuration.GetConnectionString("MSSQL"),
     sinkOptions: new MSSqlServerSinkOptions
     {
         AutoCreateSqlTable = true,
         TableName = "Logs",
     },
     appConfiguration: null,
     columnOptions: columnOpt
    )
    .Enrich.FromLogContext()
    .Enrich.With<CustomUserNameColumn>()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);

//Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();

//builder.Host.UseSerilog(((ctx, lc) => lc
//.ReadFrom.Configuration(ctx.Configuration)));

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});
builder.Services.AddMemoryCache();
//builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacBusinessModule()));


//builder.Services.LoadCore();
//builder.Services.Load();
//builder.Services.AddStorage<LocalStorage>();
//builder.Services.AddPaymentService<IyzipayAdapter>();



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

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var username = context.User?.Identity?.Name;
    LogContext.PushProperty("user_name", username);
    await next();
});

app.UseSerilogRequestLogging();

app.UseHttpLogging();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
