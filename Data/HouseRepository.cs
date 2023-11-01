using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IHouseRepository
{
    Task<List<HouseDTO>> GetAll();

    Task<HouseDetailDTO> Get(int id);

    Task<HouseDetailDTO> Add(HouseDetailDTO dto);

    Task<HouseDetailDTO> Update(HouseDetailDTO dto);

    Task Delete(int id);
}

public class HouseRepository : IHouseRepository
{
    private readonly HouseDbContext context;
    private readonly IMapper mapper;

    public HouseRepository(HouseDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<HouseDTO>> GetAll()
    {
        return await context.Houses.Select(h => new HouseDTO(h.Id, h.Address, h.Country, h.Price)).ToListAsync();
    }

    public async Task<HouseDetailDTO> Get(int id)
    {
        var result = await context.Houses.SingleOrDefaultAsync(h => h.Id == id);

        if (result == null) return null;

        return mapper.Map<HouseDetailDTO>(result);
    }

    public async Task<HouseDetailDTO> Add(HouseDetailDTO dto)
    {
        var entity = mapper.Map<HouseEntity>(dto);
        context.Houses.Add(entity);
        await context.SaveChangesAsync();

        return mapper.Map<HouseDetailDTO>(entity);
    }

    public async Task<HouseDetailDTO> Update(HouseDetailDTO dto)
    {
        var entity = await context.Houses.FindAsync(dto.Id);
        if (entity == null)
            throw new ArgumentException($"House with id {dto.Id} not found");

        var entityInput = mapper.Map<HouseEntity>(dto);
        context.Entry(entityInput).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return mapper.Map<HouseDetailDTO>(entityInput);
    }

    public async Task Delete(int id)
    {
        var entity = await context.Houses.FindAsync(id);
        if (entity == null)
            throw new ArgumentException($"House with id {id} not found");

        context.Houses.Remove(entity);
        await context.SaveChangesAsync();
    }
}