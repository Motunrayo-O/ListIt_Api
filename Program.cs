using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

builder.Services.AddDbContext<HouseDbContext>(o => o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(p => p.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/houses", (IHouseRepository repo) => repo.GetAll()).Produces<HouseDTO[]>(StatusCodes.Status200OK);

app.MapGet("/house/{houseId:int}", async (int houseId, IHouseRepository repo) =>
{
    var house = await repo.Get(houseId);
    if (house == null)
        return Results.Problem($"House with Id {houseId} not found.", statusCode: 404);

    return Results.Ok(house);
}).ProducesProblem(404).Produces<HouseDetailDTO>(StatusCodes.Status200OK);

app.MapPost("/houses", async ([FromBody] HouseDetailDTO dto, IHouseRepository repo) =>
{
    if (!MiniValidator.TryValidate(dto, out var errors))
        return Results.ValidationProblem(errors);

    var newHouse = await repo.Add(dto);
    return Results.Created($"house/{newHouse.Id}", newHouse);
}).ProducesValidationProblem().Produces<HouseDetailDTO>(StatusCodes.Status201Created);

app.MapPut("/houses", async ([FromBody] HouseDetailDTO dto, IHouseRepository repo) =>
{
    if (!MiniValidator.TryValidate(dto, out var errors))
        return Results.ValidationProblem(errors);

    if (await repo.Get(dto.Id) == null)
        return Results.Problem($"House with id {dto.Id} not found", statusCode: 404);
    var updatedHouse = await repo.Update(dto);
    return Results.Ok(updatedHouse);
}).ProducesValidationProblem().ProducesProblem(404).Produces<HouseDetailDTO>(StatusCodes.Status200OK);

app.MapDelete("/houses/{houseId:int}", async (int id, IHouseRepository repo) =>
{
    if (await repo.Get(id) == null)
        return Results.Problem($"House with id {id} not found", statusCode: 404);
    await repo.Delete(id);
    return Results.Ok();
}).ProducesProblem(404).Produces(StatusCodes.Status200OK);

app.MapGet("/house/{houseId:int}/bids", async (int houseId, IHouseRepository houseRepo, IBidRepository bidRepository) =>
{
    if (await houseRepo.Get(houseId) == null)
        return Results.Problem($"House with Id ${houseId} DoesNotReturnAttribute not exist.", statusCode: 404);
    var results = await bidRepository.GetByHouseId(houseId);

    return Results.Ok(results);
}).Produces(StatusCodes.Status200OK).ProducesProblem(StatusCodes.Status404NotFound);

app.MapPost("/house/{houseId:int}/bids", async ([FromBody] BidDTO dto, int houseId, IBidRepository bidRepository) =>
{
    if (dto.HouseId != houseId)
        return Results.Problem($"House Id does not match path.", statusCode: StatusCodes.Status400BadRequest);
    if (!MiniValidator.TryValidate(dto, out var errors))
        return Results.ValidationProblem(errors);
    var result = await bidRepository.Add(dto);
    return Results.Created($"/houses/{result.HouseId}/bids", result);
}).ProducesProblem(StatusCodes.Status400BadRequest).ProducesValidationProblem().Produces<BidDTO>(StatusCodes.Status201Created);

app.Run();