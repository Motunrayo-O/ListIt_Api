using Microsoft.EntityFrameworkCore;

public interface IHouseRepository
{
    Task<List<HouseDTO>> GetAll();
}

public class HouseRepository : IHouseRepository
{
    private readonly HouseDbContext context;

    public HouseRepository(HouseDbContext context)
    {
        this.context = context;
    }

    public async Task<List<HouseDTO>> GetAll()
    {
        return  await context.Houses.Select(h => new HouseDTO(h.Id, h.Address, h.Country, h.Price)).ToListAsync();
    }
}