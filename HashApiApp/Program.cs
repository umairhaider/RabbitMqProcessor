using HashApiApp.DAL;
using HashApiApp.RabbitMq;
using HashApiApp.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IHashRepository, HashRepository>();
builder.Services.AddSingleton(x => new RabbitMqProducer(builder.Configuration.GetValue<string>("RabbitMq:ConnectionString")));

builder.Services.AddDbContext<HashDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HashDatabase")));
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
