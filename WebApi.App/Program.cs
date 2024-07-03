using DB.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using WebApi.App.Hubs;
using WebApi.App.Services;
using WebApi.App.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => // avoid the cyclic calls in JSON serialization
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// inject DBContext
builder.Services.AddDbContext<ChatDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"))
);

// add signalR
builder.Services.AddSignalR();

// inject Utils to use the methods
builder.Services.AddSingleton<Utilities>();

// implement Auto Mapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// inject services
builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddScoped<IChatService, ChatService>();

// Add the Authentication indicating that use JWT.
builder.Services.AddAuthentication(config =>
{
    // add authentication schemes
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config => // Add the JWT, set configuration parameters to JwtBearer
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false, // validate if can access to the urls, in this we put false
        ValidateAudience = false, // validate who can access to the urls
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero, // avoid issues with an existing delay in Clock Hour.
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!) // get the key in appsettings, converting in bytes
            )
    };
});

// set api urls to lowercase
builder.Services.AddRouting(routing => routing.LowercaseUrls = true);

// add CORS options, creatiing a new default cors Policy.
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", app =>
    {
        app.WithOrigins("http://127.0.0.1:5500") // the url that access to server
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// indicate that use cors
app.UseCors("MyPolicy");

app.UseHttpsRedirection();

// indicate that use the Authentication.
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// map the Hub created, and set the url
app.MapHub<ChatHub>("/chatHub");

app.Run();
