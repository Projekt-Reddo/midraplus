using System.Net;
using AccountService.Data;
using AccountService.Dtos;
using AccountService.Helpers;
using AccountService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

// MongoDbContext
builder.Services.Configure<MongoDbSetting>(builder.Configuration.GetSection("MongoDbSetting"));
builder.Services.AddSingleton<MongoDbSetting>(sp => sp.GetRequiredService<IOptions<MongoDbSetting>>().Value);
builder.Services.AddSingleton<IMongoContext, MongoContext>();

// Auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repository
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<ISignInRepo, SignInRepo>();

// alows CORS
//builder.Services.AddCors();

// Authentication
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSecret"]))
    };
});

// Authorization
builder.Services.AddAuthorization();

// JWT 
builder.Services.AddSingleton<IJwtGenerator>(new JwtGenerator(configuration["JwtSecret"]));

// rabbitMQ service
builder.Services.AddScoped<IMessageBusPublisher, MessageBusPublisher>();

// Grpc Server
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token (Access Token) on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    opt.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

#endregion

#region App pipeline

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // For testing Grpc
    app.MapGrpcReflectionService();
}

// Cause error when using nginx
//app.UseHttpsRedirection();

// cors has to be on top of all
app.UseCors(opt => opt.WithOrigins(builder.Configuration.GetSection("FrontendUrl").Get<string[]>())
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials());

app.UseExceptionHandler(e => e.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()!.Error;
    await context.Response.WriteAsJsonAsync(new ResponseDto(500, exception.Message));
}));

app.Use(async (context, next) =>
{
    await next();

    // 401 - Unauthorized
    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseDto(401), new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
    }

    // 403 - Forbidden
    if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(new ResponseDto(403), new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcUserServer>();

app.Run();

#endregion