using System.ComponentModel.DataAnnotations;

public record BidDTO(int Id, int HouseId, [property: Required]string Bidder, int Amount);