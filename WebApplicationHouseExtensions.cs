using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationHouseExtensions
{
    public static void MapHouseEndPoints(this WebApplication app)
    {
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
    }
}