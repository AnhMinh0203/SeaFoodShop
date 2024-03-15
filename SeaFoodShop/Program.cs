using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SeaFoodShop.API.Controllers;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.Repository;
using SeaFoodShop.Repository.Interface;
using System.Text;
using Twilio.Clients;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Jwt Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// SMS
/*builder.Services.AddHttpClient<ITwilioRestClient, TwilioClient>();*/

// Register service
builder.Services.AddSingleton<ConnectToSql>();
builder.Services.AddScoped<IAccountRespon, AccountRespon>();
builder.Services.AddScoped<SeaFoodRespon>();
builder.Services.AddScoped<CommentRespon>();
builder.Services.AddScoped<ShoppingCartRespon>();
builder.Services.AddScoped<TypeRespon>();
builder.Services.AddScoped<UserRespon>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
