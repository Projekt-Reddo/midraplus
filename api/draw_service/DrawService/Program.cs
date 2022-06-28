using DrawService.Dtos;
using DrawService.Hubs;
using DrawService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

// SignalR
builder.Services.AddSignalR();

// Store users connected to the board
builder.Services.AddSingleton<IDictionary<string, DrawConnection>>(opt => new Dictionary<string, DrawConnection>());
// Store users connected to the chat
builder.Services.AddSingleton<IDictionary<string, ChatConnection>>(opt => new Dictionary<string, ChatConnection>());
// Dict between Room: Shapes of rooms
builder.Services.AddSingleton<IDictionary<string, ICollection<ShapeReadDto>>>(opt => new Dictionary<string, ICollection<ShapeReadDto>>());
// Dict between Room: Notes of rooms
builder.Services.AddSingleton<IDictionary<string, List<NoteReadDto>>>(opt => new Dictionary<string, List<NoteReadDto>>());

// Auto mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// alows CORS
builder.Services.AddCors();

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

// Cause error when using nginx
// app.UseHttpsRedirection();

// cors has to be on top of all
app.UseCors(opt => opt.WithOrigins(builder.Configuration.GetSection("FrontendUrl").Get<string[]>())
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();

// app.MapControllers();

// SignalR endpoints
app.MapHub<BoardHub>("/board");
app.MapHub<ChatHub>("/chat");

app.Run();

#endregion