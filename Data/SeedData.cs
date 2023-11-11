using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Seed(ModelBuilder builder)
    {
        builder.Entity<HouseEntity>().HasData(new List<HouseEntity>{
            new() {
                Id = 1,
                Address = " 5 Sweet Valley High, Gloryland",
                Country = "Razania",
                Description = "A bright spacious and modern property in the much sought after SVH area of Gloryland",
                Price = 450000
            },
            new ()
            {
                Id = 2,
                Address = "89 Road of Forks, Bern",
                Country = "Switzerland",
                Description = "This impressive family home, which dates back to approximately 1840, offers original period features throughout and is set back from the road with off street parking for up to six cars and an original Coach House, which has been incorporated into the main house to provide further accommodation. ",
                Price = 500000
            },
            new ()
            {
                Id = 3,
                Address = "Grote Hof 12, Amsterdam",
                Country = "The Netherlands",
                Description = "This house has been designed and built to an impeccable standard offering luxurious and elegant living. The accommodation is arranged over four floors comprising a large entrance hall, living room with tall sash windows, dining room, study and WC on the ground floor.",
                Price = 200500
            },
            new ()
            {
                Id = 4,
                Address = "Meel Kade 321, The Hague",
                Country = "The Netherlands",
                Description = "Discreetly situated a unique two storey period home enviably located on the corner of Krom Road and Recht Road offering seclusion and privacy. The house features a magnificent double height reception room with doors leading directly out onto a charming courtyard garden.",
                Price = 259500
            },
            new ()
            {
                Id = 5,
                Address = "Oude Gracht 32, Utrecht",
                Country = "The Netherlands",
                Description = "This luxurious three bedroom flat is contemporary in style and benefits from the use of a gymnasium and a reserved underground parking space.",
                Price = 400500
            }
        });

        builder.Entity<BidEntity>().HasData(new List<BidEntity>
        {new BidEntity { Id = 1, HouseId = 1, Bidder = "Sonia Reading", Amount = 200000 },
            new BidEntity { Id = 2, HouseId = 1, Bidder = "Dick Johnson", Amount = 202400 },
            new BidEntity { Id = 3, HouseId = 2, Bidder = "Mohammed Vahls", Amount = 302400 },
            new BidEntity { Id = 4, HouseId = 2, Bidder = "Jane Williams", Amount = 310500 },
            new BidEntity { Id = 5, HouseId = 2, Bidder = "John Kepler", Amount = 315400 },
            new BidEntity { Id = 6, HouseId = 3, Bidder = "Bill Mentor", Amount = 201000 },
            new BidEntity { Id = 7, HouseId = 4, Bidder = "Melissa Kirk", Amount = 410000 },
            new BidEntity { Id = 8, HouseId = 4, Bidder = "Scott Max", Amount = 450000 },
            new BidEntity { Id = 9, HouseId = 4, Bidder = "Christine James", Amount = 470000 },
            new BidEntity { Id = 10, HouseId = 5, Bidder = "Omesh Carim", Amount = 450000 },
            new BidEntity { Id = 11, HouseId = 5, Bidder = "Charlotte Max", Amount = 150000 },
            new BidEntity { Id = 12, HouseId = 5, Bidder = "Marcus Scott", Amount = 170000 },
        });
    }
}