using FrisbeeApp.Context;
using FrisbeeApp.Controllers.Mappers;
using FrisbeeApp.DatabaseModels.Models;
using FrisbeeApp.Logic.Abstractisations;
using FrisbeeApp.Logic.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<FrisbeeAppContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString") ?? throw new InvalidOperationException("Connection string SQLConnectioString not found")));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<FrisbeeAppContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddAutoMapper(typeof(LoginApiModelProfile));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
