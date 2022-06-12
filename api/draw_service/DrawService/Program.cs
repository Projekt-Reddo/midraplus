using DrawService.Dtos;
using DrawService.Hubs;
using DrawService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

// SignalR
builder.Services.AddSignalR();
builder.Services.AddSingleton<IDictionary<string, DrawConnection>>(opt => new Dictionary<string, DrawConnection>());
builder.Services.AddSingleton<IDictionary<string, ICollection<ShapeReadDto>>>(opt => new Dictionary<string, ICollection<ShapeReadDto>>());
builder.Services.AddSingleton<IDictionary<string, List<NoteReadDto>>>(opt => new Dictionary<string, List<NoteReadDto>>());

// Auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Grpc Clients
builder.Services.AddScoped<IGrpcBoardClient, GrpcBoardClient>();

// Authentication
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

// builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

#endregion

#region App pipeline

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

// app.MapControllers();

// SignalR
app.MapHub<BoardHub>("/board");
app.MapHub<ChatHub>("/chat");
app.Run();

#endregion