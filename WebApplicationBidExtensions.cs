using Microsoft.AspNetCore.Mvc;
using MiniValidation;

public static class WebApplicationBidExtensions
{
    public static void MapBidEndPoints(this WebApplication app)
    {
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
    }
}