using AutoMapper;
using ChessMobileBE.Contracts;
using ChessMobileBE.Map;
using ChessMobileBE.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
{
    var uri = s.GetRequiredService<IConfiguration>()["MongoUri"];
    return new MongoClient(uri);
});
var config = new MapperConfiguration(cfr =>
{
    cfr.AddProfile(new AutoMapperProfile());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IPendingMatchService, PendingMatchService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
