using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IBidRepository
{
    Task<List<BidDTO>> GetByHouseId(int houseId);


    Task<BidDTO> Add(BidDTO dto);
}

public class BidRepository : IBidRepository
{
    private HouseDbContext context;
    private IMapper mapper;

    public BidRepository(HouseDbContext dbContext, IMapper mapper)
    {
        this.context = dbContext;
        this.mapper = mapper;
    }
    public async Task<BidDTO> Add(BidDTO dto)
    {
        var entity = mapper.Map<BidEntity>(dto);
        context.Bids.Add(entity);
        await context.SaveChangesAsync();

        return mapper.Map<BidDTO>(entity);
    }

    public Task<List<BidDTO>> GetByHouseId(int houseId)
    {
        return context.Bids.Where(bid => bid.HouseId == houseId).Select(e => mapper.Map<BidDTO>(e)).ToListAsync();
    }
}
